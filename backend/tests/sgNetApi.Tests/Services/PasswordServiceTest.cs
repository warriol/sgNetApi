using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using sgNetApi.Domain.DTOs;
using sgNetApi.Domain.Entities;
using sgNetApi.Domain.Interfaces;
using sgNetApi.Infrastructure.Data;
using sgNetApi.Infrastructure.Services;
using Xunit;
using Xunit.Abstractions;

namespace sgNetApi.Tests.Services;

public class PasswordServiceTests
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasherMock;
    private readonly PasswordService _service;
    private readonly ITestOutputHelper _output;

    public PasswordServiceTests(ITestOutputHelper output)
    {
        _output = output;

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _passwordHasherMock = Substitute.For<IPasswordHasher>();

        _service = new PasswordService(_context, _passwordHasherMock);
    }

    [Fact]
    public async Task CambiarPassword_DeberiaFallar_CuandoNuevaClaveCoincideConAlgunaDeLasUltimas5()
    {
        _output.WriteLine("=== INICIO TEST: Rechazar clave si coincide con las últimas 5 ===");

        // Arrange
        long ci = 12345678;
        _output.WriteLine($"[1. ARRANGE] Inicializando usuario con CI: {ci}");

        var usuario = new Usuario
        {
            Ci = ci,
            NombreUsuario = "12345678",
            Nombre = "Prueba",
            Apellido = "Test",
            Correo = "test@sgnet.gub.uy",
            PasswordHash = new byte[] { 1, 2, 3 },
            PasswordSalt = new byte[] { 4, 5, 6 },
            Habilitado = true,
            IdGrado = 1, IdEscalafon = 1, IdUuee = 1, IdDependencia = 1
        };

        _context.Usuarios.Add(usuario);

        _output.WriteLine("[1. ARRANGE] Insertando 5 registros previos en el historial de contraseñas...");
        for (int i = 1; i <= 5; i++)
        {
            _context.HistorialesPasswords.Add(new HistorialPassword
            {
                UsuarioCi = ci,
                PasswordHash = new byte[] { (byte)i },
                FechaCreacion = DateTime.UtcNow.AddDays(-i)
            });
        }
        await _context.SaveChangesAsync();

        _passwordHasherMock.VerificarPasswordHash("ClaveActual.123", usuario.PasswordHash, usuario.PasswordSalt)
            .Returns(true);

        // Simulamos que la nueva clave coincide con el historial
        _passwordHasherMock.VerificarPasswordHash("ClaveRepetida.123", Arg.Any<byte[]>(), usuario.PasswordSalt)
            .Returns(true);

        var dto = new CambiarPasswordDto
        {
            Ci = ci,
            PasswordActual = "ClaveActual.123",
            PasswordNueva = "ClaveRepetida.123"
        };

        // Act
        _output.WriteLine("[2. ACT] Invocando CambiarPasswordAsync con una contraseña repetida...");
        var (exito, mensaje) = await _service.CambiarPasswordAsync(dto);

        // Assert
        _output.WriteLine($"[3. ASSERT] Resultado obtenido -> Éxito: {exito} | Mensaje: '{mensaje}'");

        exito.Should().BeFalse();
        mensaje.Should().Contain("últimas 5 contraseñas");

        _output.WriteLine("=== FIN TEST: Prueba ejecutada y validada correctamente ===\n");
    }

    [Fact]
    public async Task CambiarPassword_DeberiaTenerExito_CuandoNuevaClaveEsDiferenteALasUltimas5()
    {
        _output.WriteLine("=== INICIO TEST: Permitir cambio de clave cuando es totalmente nueva ===");

        // Arrange
        long ci = 12345678;
        _output.WriteLine($"[1. ARRANGE] Inicializando usuario con CI: {ci}");

        var usuario = new Usuario
        {
            Ci = ci,
            NombreUsuario = "12345678",
            Nombre = "Prueba",
            Apellido = "Test",
            Correo = "test@sgnet.gub.uy",
            PasswordHash = new byte[] { 1, 2, 3 },
            PasswordSalt = new byte[] { 4, 5, 6 },
            Habilitado = true,
            IdGrado = 1, IdEscalafon = 1, IdUuee = 1, IdDependencia = 1
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        _passwordHasherMock.VerificarPasswordHash("ClaveActual.123", usuario.PasswordHash, usuario.PasswordSalt)
            .Returns(true);

        // Simulamos que la nueva clave NO coincide con el historial
        _passwordHasherMock.VerificarPasswordHash("ClaveNuevaValida.123", Arg.Any<byte[]>(), usuario.PasswordSalt)
            .Returns(false);

        // Configurar el mock para asignar los arrays out de nuevoHash y nuevoSalt
        _passwordHasherMock.When(x => x.CrearPasswordHash(Arg.Any<string>(), out Arg.Any<byte[]>()!, out Arg.Any<byte[]>()!))
            .Do(callInfo =>
            {
                callInfo[1] = new byte[] { 10, 20, 30 }; // Asigna valor al parámetro out nuevoHash
                callInfo[2] = new byte[] { 40, 50, 60 }; // Asigna valor al parámetro out nuevoSalt
            });

        var dto = new CambiarPasswordDto
        {
            Ci = ci,
            PasswordActual = "ClaveActual.123",
            PasswordNueva = "ClaveNuevaValida.123"
        };

        // Act
        _output.WriteLine("[2. ACT] Invocando CambiarPasswordAsync con una contraseña nueva y válida...");
        var (exito, mensaje) = await _service.CambiarPasswordAsync(dto);

        // Assert
        _output.WriteLine($"[3. ASSERT] Resultado obtenido -> Éxito: {exito} | Mensaje: '{mensaje}'");

        exito.Should().BeTrue();
        mensaje.Should().Be("La contraseña ha sido actualizada correctamente.");

        var registrosHistorial = await _context.HistorialesPasswords.Where(h => h.UsuarioCi == ci).ToListAsync();
        _output.WriteLine($"[3. ASSERT] Verificando historial -> Registros grabados en BD: {registrosHistorial.Count}");
        
        registrosHistorial.Should().HaveCount(1);

        _output.WriteLine("=== FIN TEST: Prueba ejecutada y validada correctamente ===\n");
    }
}