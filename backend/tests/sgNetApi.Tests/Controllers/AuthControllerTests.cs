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

namespace sgNetApi.Tests.Controllers;

public class AuthControllerTests
{
    [Fact]
    public async Task Login_BloqueaLaCuenta_AlTercerIntentoFallido()
    {
        await using var context = CrearContexto();
        var hasher = Substitute.For<IPasswordHasher>();
        var controller = CrearController(context, hasher);
        var usuario = CrearUsuario();
        usuario.IntentosFallidos = 2;
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();
        hasher.VerificarPasswordHash("incorrecta", usuario.PasswordHash).Returns(false);

        var response = await controller.Login(new LoginRequestDto { NombreUsuario = usuario.NombreUsuario, Password = "incorrecta" });

        response.Should().BeOfType<UnauthorizedObjectResult>();
        var guardado = await context.Usuarios.SingleAsync();
        guardado.IntentosFallidos.Should().Be(3);
        guardado.Habilitado.Should().BeFalse();
    }

    [Fact]
    public async Task Login_ReiniciaIntentosFallidos_CuandoEsValido()
    {
        await using var context = CrearContexto();
        var hasher = Substitute.For<IPasswordHasher>();
        var token = Substitute.For<IJwtTokenGenerator>();
        var controller = new AuthController(context, hasher, token, Substitute.For<IPasswordService>());
        var usuario = CrearUsuario();
        usuario.IntentosFallidos = 2;
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();
        hasher.VerificarPasswordHash("correcta", usuario.PasswordHash).Returns(true);
        token.GenerarToken(usuario, Arg.Any<List<string>>(), Arg.Any<List<string>>()).Returns("token");

        var response = await controller.Login(new LoginRequestDto { NombreUsuario = usuario.NombreUsuario, Password = "correcta" });

        response.Should().BeOfType<OkObjectResult>();
        (await context.Usuarios.SingleAsync()).IntentosFallidos.Should().Be(0);
    }

    private static AuthController CrearController(AppDbContext context, IPasswordHasher hasher) =>
        new(context, hasher, Substitute.For<IJwtTokenGenerator>(), Substitute.For<IPasswordService>());

    private static Usuario CrearUsuario() => new()
    {
        NombreUsuario = "123456789", Ci = 123456789, Nombre = "Prueba", Apellido = "Test",
        Correo = "test@sgnet.gub.uy", PasswordHash = "hash-actual", IdNacionalidad = 1
    };

    private static AppDbContext CrearContexto() => new(new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
}
