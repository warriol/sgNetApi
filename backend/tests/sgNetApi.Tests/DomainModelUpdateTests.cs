using Microsoft.EntityFrameworkCore;
using sgNetApi.Domain.Entities;
using sgNetApi.Infrastructure.Data;

namespace sgNetApi.Tests;

public class DomainModelUpdateTests
{
    [Fact]
    public async Task CanCreateCoreDomainEntitiesAndPersistThem()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);

        var uuee = new UnidadEjecutora { IdUuee = 1, Nombre = "Unidad Ejecutora A", Siglas = "UUEE-A" };
        var direccion = new Direccion
        {
            TipoDireccion = 3,
            Pais = "Uruguay",
            Departamento = "Montevideo",
            Localidad = "Montevideo",
            Calle = "Av. 18 de Julio",
            Numero = "1234",
            CodigoPostal = "11100",
            Telefono = "099000000"
        };

        var turno = new Turno
        {
            Nombre = "Mañana",
            HoraInicio = new TimeOnly(8, 0),
            HoraFin = new TimeOnly(16, 0),
            Descripcion = "Turno diurno",
            Habilitado = true
        };

        var dependencia = new Dependencia
        {
            IdUuee = 1,
            Nombre = "Dependencia Operativa",
            Siglas = "DO",
            UnidadEjecutora = uuee,
            Direccion = direccion,
            IdTurno = 1,
            NombreUsuarioJefe = "user-1",
            NombreUsuarioSegundoJefe = "user-2"
        };

        var nacionalidad = new Nacionalidad { IdNacionalidad = 1, Nombre = "Uruguaya", CodigoIso = "UY", EsUruguaya = true };
        var usuario1 = new Usuario
        {
            NombreUsuario = "user-1",
            Correo = "user1@sgnet.com.uy",
            Nombre = "Usuario",
            Apellido = "Uno",
            PasswordHash = "hash",
            FechaNacimiento = new DateOnly(1990, 1, 1),
            IdNacionalidad = 1,
            Nacionalidad = nacionalidad,
            IdDependencia = 1,
            Dependencia = dependencia
        };

        var usuario2 = new Usuario
        {
            NombreUsuario = "user-2",
            Correo = "user2@sgnet.com.uy",
            Nombre = "Usuario",
            Apellido = "Dos",
            PasswordHash = "hash",
            FechaNacimiento = new DateOnly(1991, 2, 2),
            IdNacionalidad = 1,
            Nacionalidad = nacionalidad,
            IdDependencia = 1,
            Dependencia = dependencia
        };

        var arma = new ArmaPolicial
        {
            IdArmaTipo = 1,
            IdArmaMarca = 1,
            IdArmaModelo = 1,
            Serie = "ABC123",
            Numero = "AR-01",
            FechaEntrega = DateTime.UtcNow,
            IdArmaEstado = 1,
            Observaciones = "Disponible"
        };

        var chaleco = new ChalecoAntibalaPolicial
        {
            IdChalecoTipo = 1,
            IdChalecoMarca = 1,
            IdChalecoModelo = 1,
            IdChalecoTalle = 1,
            Color = "Negro",
            FechaEntrega = DateTime.UtcNow,
            FechaVencimiento = DateTime.UtcNow.AddYears(2),
            IdChalecoEstado = 1,
            Observaciones = "En buen estado"
        };

        var esposas = new EsposasPolicial
        {
            IdEsposasTipo = 1,
            IdEsposasMarca = 1,
            IdEsposasModelo = 1,
            FechaEntrega = DateTime.UtcNow,
            Observaciones = "Listas para uso"
        };

        var asignacion = new FuncionarioEquipoPolicial
        {
            NombreUsuario = "user-1",
            IdArmaPolicial = 1,
            IdChalecoAntibalaPolicial = 1,
            IdEsposasPolicial = 1,
            FechaAsignacion = DateTime.UtcNow,
            Observaciones = "Asignación inicial"
        };

        context.UnidadesEjecutoras.Add(uuee);
        context.Direcciones.Add(direccion);
        context.Turnos.Add(turno);
        context.Dependencias.Add(dependencia);
        context.Nacionalidades.Add(nacionalidad);
        context.Usuarios.AddRange(usuario1, usuario2);
        context.ArmasPoliciales.Add(arma);
        context.ChalecosAntibalaPoliciales.Add(chaleco);
        context.EsposasPoliciales.Add(esposas);
        context.FuncionariosEquipoPolicial.Add(asignacion);

        await context.SaveChangesAsync();

        Assert.Equal(2, await context.Usuarios.CountAsync());
        Assert.Equal(1, await context.Dependencias.CountAsync());
        Assert.Equal(1, await context.FuncionariosEquipoPolicial.CountAsync());
    }
}
