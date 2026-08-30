namespace sgNetApi.Domain.Entities;

public class Nacionalidad
{
    public int IdNacionalidad { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string CodigoIso { get; set; } = string.Empty;
    public bool EsUruguaya { get; set; }
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}

public class EstadoCivil
{
    public int IdEstadoCivil { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class Profesion
{
    public int IdProfesion { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class Grado
{
    public int IdGrado { get; set; }
    public int Numero { get; set; }
    public string Texto { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
}

public class Escalafon
{
    public int IdEscalafon { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
}

public class UnidadEjecutora
{
    public int IdUuee { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Siglas { get; set; } = string.Empty;
}

public class Direccion
{
    public int IdDireccion { get; set; }
    public int TipoDireccion { get; set; }
    public string Pais { get; set; } = string.Empty;
    public string Departamento { get; set; } = string.Empty;
    public string Localidad { get; set; } = string.Empty;
    public string? Calle { get; set; }
    public string? Cruce1 { get; set; }
    public string? Cruce2 { get; set; }
    public string? Numero { get; set; }
    public string? Apartamento { get; set; }
    public string? Manzana { get; set; }
    public string? Solar { get; set; }
    public string? Ruta { get; set; }
    public decimal? Km { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string? CodigoPostal { get; set; }
    public string? Telefono { get; set; }
}

public class Turno
{
    public int IdTurno { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string TipoTurno { get; set; } = "4x6";
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }
    public string? Descripcion { get; set; }
    public bool Habilitado { get; set; } = true;
    public ICollection<Dependencia> Dependencias { get; set; } = new List<Dependencia>();
}

public static class TurnoHelper
{
    public static TimeOnly CalcularHoraFin(string tipoTurno, TimeOnly horaInicio)
    {
        var horasTipo = tipoTurno switch
        {
            "4x6" => 6,
            "3x8" => 8,
            "2x12" => 12,
            "1x24" => 24,
            _ => 24
        };

        var totalMinutos = (horasTipo * 60) - 1;
        var fecha = DateTime.Today.Add(horaInicio.ToTimeSpan()).AddMinutes(totalMinutos);
        return TimeOnly.FromTimeSpan(fecha.TimeOfDay);
    }
}

public class Dependencia
{
    public int IdDependencia { get; set; }

    public int IdUuee { get; set; }
    public UnidadEjecutora UnidadEjecutora { get; set; } = null!;

    public string Nombre { get; set; } = string.Empty;
    public string Siglas { get; set; } = string.Empty;

    public int? IdDireccion { get; set; }
    public Direccion? Direccion { get; set; }

    public string? NombreUsuarioJefe { get; set; }
    public string? NombreUsuarioSegundoJefe { get; set; }

    public int? IdTurno { get; set; }
    public Turno? Turno { get; set; }

    public ICollection<Usuario> Funcionarios { get; set; } = new List<Usuario>();
}

public class ArmaTipo
{
    public int IdArmaTipo { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class ArmaMarca
{
    public int IdArmaMarca { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class ArmaModelo
{
    public int IdArmaModelo { get; set; }
    public int IdArmaTipo { get; set; }
    public int IdArmaMarca { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public ArmaTipo? ArmaTipo { get; set; }
    public ArmaMarca? ArmaMarca { get; set; }
}

public class ArmaEstado
{
    public int IdArmaEstado { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class ArmaPolicial
{
    public int IdArmaPolicial { get; set; }
    public int IdArmaTipo { get; set; }
    public int IdArmaMarca { get; set; }
    public int IdArmaModelo { get; set; }
    public string? Serie { get; set; }
    public string? Numero { get; set; }
    public DateTime? FechaEntrega { get; set; }
    public int IdArmaEstado { get; set; }
    public string? Observaciones { get; set; }

    public ArmaTipo? ArmaTipo { get; set; }
    public ArmaMarca? ArmaMarca { get; set; }
    public ArmaModelo? ArmaModelo { get; set; }
    public ArmaEstado? ArmaEstado { get; set; }
}

public class ChalecoTipo
{
    public int IdChalecoTipo { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class ChalecoMarca
{
    public int IdChalecoMarca { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class ChalecoModelo
{
    public int IdChalecoModelo { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class ChalecoTalle
{
    public int IdChalecoTalle { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class ChalecoEstado
{
    public int IdChalecoEstado { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class ChalecoAntibalaPolicial
{
    public int IdChalecoAntibalaPolicial { get; set; }
    public int IdChalecoTipo { get; set; }
    public int IdChalecoMarca { get; set; }
    public int IdChalecoModelo { get; set; }
    public int IdChalecoTalle { get; set; }
    public string? Color { get; set; }
    public DateTime? FechaEntrega { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public int IdChalecoEstado { get; set; }
    public string? Observaciones { get; set; }
}

public class EsposasTipo
{
    public int IdEsposasTipo { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class EsposasMarca
{
    public int IdEsposasMarca { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class EsposasModelo
{
    public int IdEsposasModelo { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class EsposasPolicial
{
    public int IdEsposasPolicial { get; set; }
    public int IdEsposasTipo { get; set; }
    public int IdEsposasMarca { get; set; }
    public int IdEsposasModelo { get; set; }
    public DateTime? FechaEntrega { get; set; }
    public string? Observaciones { get; set; }
}

public class FuncionarioEquipoPolicial
{
    public int IdFuncionarioEquipoPolicial { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public int IdArmaPolicial { get; set; }
    public int IdChalecoAntibalaPolicial { get; set; }
    public int IdEsposasPolicial { get; set; }
    public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaDevolucion { get; set; }
    public string? Observaciones { get; set; }

    public Usuario? Usuario { get; set; }
    public ArmaPolicial? ArmaPolicial { get; set; }
    public ChalecoAntibalaPolicial? ChalecoAntibalaPolicial { get; set; }
    public EsposasPolicial? EsposasPolicial { get; set; }
}