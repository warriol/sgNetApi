using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using sgNetApi.Api.Controllers;
using sgNetApi.Domain.DTOs;
using sgNetApi.Domain.Entities;
using sgNetApi.Domain.Interfaces;
using sgNetApi.Infrastructure.Data;
using Xunit;
using Xunit.Abstractions;

namespace sgNetApi.Tests.Controllers;

public class AuthControllerTests
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasherMock;
    private readonly IJwtTokenGenerator _jwtTokenGeneratorMock;
    private readonly IPasswordService _passwordServiceMock;
    private readonly AuthController _controller;
    private readonly ITestOutputHelper _output;

    public AuthControllerTests(ITestOutputHelper output)
    {
        _output = output;

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _passwordHasherMock = Substitute.For<IPasswordHasher>();
        _jwtTokenGeneratorMock = Substitute.For<IJwtTokenGenerator>();
        _passwordServiceMock = Substitute.For<IPasswordService>();

        _controller = new AuthController(
            _context,
            _passwordHasherMock,
            _jwtTokenGeneratorMock,
            _passwordServiceMock
        );
    }

    [Fact]
    public async Task Login_DeberiaBloquearCuenta_AlTercerIntentoFallidoConsecutivo()
    {
        _output.WriteLine("=== INICIO TEST: Bloqueo automático de cuenta tras 3 intentos fallidos ===");

        // 1. ARRANGE
        long ci = 12345678;
        _output.WriteLine($"[1. ARRANGE] Creando usuario con CI: {ci} con 2 intentos fallidos previos...");

        var usuario = new Usuario
        {
            Ci = ci,
            NombreUsuario = "12345678",
            Nombre = "Prueba",
            Apellido = "Bloqueo",
            Correo = "bloqueo@sgnet.gub.uy",
            PasswordHash = new byte[] { 1, 2, 3 },
            PasswordSalt = new byte[] { 4, 5, 6 },
            Habilitado = true,
            IntentosFallidos = 2, // Ya tenía 2 fallos registrados
            ExpiradoPorInactividad = false,
            IdGrado = 1, IdEscalafon = 1, IdUuee = 1, IdDependencia = 1
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        // Configurar Mock: la clave ingresada no es válida
        _passwordHasherMock.VerificarPasswordHash("ClaveIncorrecta.123", usuario.PasswordHash, usuario.PasswordSalt)
            .Returns(false);

        var loginDto = new LoginRequestDto
        {
            Ci = ci,
            Password = "ClaveIncorrecta.123"
        };

        // 2. ACT
        _output.WriteLine("[2. ACT] Invocando Login por 3ª vez con credenciales erróneas...");
        var response = await _controller.Login(loginDto);

        // 3. ASSERT
        _output.WriteLine("[3. ASSERT] Verificando respuesta HTTP y estado del usuario en BD...");

        var result = response as BadRequestObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(400);

        var usuarioEnBd = await _context.Usuarios.FirstOrDefaultAsync(u => u.Ci == ci);
        usuarioEnBd.Should().NotBeNull();

        _output.WriteLine($"[3. ASSERT] Intentos fallidos en BD: {usuarioEnBd!.IntentosFallidos}");
        _output.WriteLine($"[3. ASSERT] Cuenta habilitada: {usuarioEnBd.Habilitado}");

        usuarioEnBd.IntentosFallidos.Should().Be(3);
        usuarioEnBd.Habilitado.Should().BeFalse("la cuenta debe haber sido deshabilitada automáticamente al llegar a 3 fallos");

        _output.WriteLine("=== FIN TEST: Bloqueo automático verificado correctamente ===\n");
    }

    [Fact]
    public async Task Login_DeberiaReiniciarIntentosFallidos_CuandoElLoginEsExitoso()
    {
        _output.WriteLine("=== INICIO TEST: Reiniciar contador de intentos al ingresar con éxito ===");

        // 1. ARRANGE
        long ci = 87654321;
        _output.WriteLine($"[1. ARRANGE] Creando usuario con CI: {ci} y 2 intentos fallidos acumulados...");

        var usuario = new Usuario
        {
            Ci = ci,
            NombreUsuario = "87654321",
            Nombre = "Prueba",
            Apellido = "Exito",
            Correo = "exito@sgnet.gub.uy",
            PasswordHash = new byte[] { 1, 2, 3 },
            PasswordSalt = new byte[] { 4, 5, 6 },
            Habilitado = true,
            IntentosFallidos = 2,
            IdGrado = 1, IdEscalafon = 1, IdUuee = 1, IdDependencia = 1
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        _passwordHasherMock.VerificarPasswordHash("ClaveCorrecta.123", usuario.PasswordHash, usuario.PasswordSalt)
            .Returns(true);

        _jwtTokenGeneratorMock.GenerarToken(usuario, Arg.Any<List<string>>(), Arg.Any<List<string>>())
            .Returns("token_jwt_mock");

        var loginDto = new LoginRequestDto
        {
            Ci = ci,
            Password = "ClaveCorrecta.123"
        };

        // 2. ACT
        _output.WriteLine("[2. ACT] Invocando Login con la contraseña correcta...");
        var response = await _controller.Login(loginDto);

        // 3. ASSERT
        var result = response as OkObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);

        var usuarioEnBd = await _context.Usuarios.FirstOrDefaultAsync(u => u.Ci == ci);
        _output.WriteLine($"[3. ASSERT] Intentos fallidos tras login exitoso: {usuarioEnBd!.IntentosFallidos}");

        usuarioEnBd.IntentosFallidos.Should().Be(0, "el contador debe volver a cero tras un login válido");

        _output.WriteLine("=== FIN TEST: Contador reseteado exitosamente ===\n");
    }
}