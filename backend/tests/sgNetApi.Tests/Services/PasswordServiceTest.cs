using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using sgNetApi.Domain.DTOs;
using sgNetApi.Domain.Entities;
using sgNetApi.Domain.Interfaces;
using sgNetApi.Infrastructure.Data;
using sgNetApi.Infrastructure.Services;
using Xunit;

namespace sgNetApi.Tests.Services;

public class PasswordServiceTests
{
    [Fact]
    public async Task CambiarPassword_RechazaUnaClaveDeLasUltimas5()
    {
        await using var context = CrearContexto();
        var hasher = Substitute.For<IPasswordHasher>();
        var service = new PasswordService(context, hasher);
        var usuario = CrearUsuario();
        context.Usuarios.Add(usuario);
        context.HistorialesPasswords.Add(new HistorialPassword { UsuarioNombreUsuario = usuario.NombreUsuario, PasswordHash = "hash-anterior" });
        await context.SaveChangesAsync();

        hasher.VerificarPasswordHash("actual", usuario.PasswordHash).Returns(true);
        hasher.VerificarPasswordHash("repetida", "hash-anterior").Returns(true);

        var resultado = await service.CambiarPasswordAsync(new CambiarPasswordDto
        {
            NombreUsuario = usuario.NombreUsuario, PasswordActual = "actual", PasswordNueva = "repetida"
        });

        resultado.Exito.Should().BeFalse();
        resultado.Mensaje.Should().Contain("últimas 5");
    }

    [Fact]
    public async Task CambiarPassword_GuardaElHashNuevo()
    {
        await using var context = CrearContexto();
        var hasher = Substitute.For<IPasswordHasher>();
        var service = new PasswordService(context, hasher);
        var usuario = CrearUsuario();
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();

        hasher.VerificarPasswordHash("actual", usuario.PasswordHash).Returns(true);
        hasher.VerificarPasswordHash("nueva", Arg.Any<string>()).Returns(false);
        hasher.CrearPasswordHash("nueva").Returns("hash-nuevo");

        var resultado = await service.CambiarPasswordAsync(new CambiarPasswordDto
        {
            NombreUsuario = usuario.NombreUsuario, PasswordActual = "actual", PasswordNueva = "nueva"
        });

        resultado.Exito.Should().BeTrue();
        (await context.HistorialesPasswords.SingleAsync()).PasswordHash.Should().Be("hash-nuevo");
    }

    private static Usuario CrearUsuario() => new()
    {
        NombreUsuario = "123456789", Ci = 123456789, Nombre = "Prueba", Apellido = "Test",
        Correo = "test@sgnet.gub.uy", PasswordHash = "hash-actual", IdNacionalidad = 1
    };

    private static AppDbContext CrearContexto() => new(new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
}
