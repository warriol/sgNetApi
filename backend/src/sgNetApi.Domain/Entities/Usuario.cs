namespace sgNetApi.Domain.Entities;

public class Usuario
{
    public string NombreUsuario { get; set; } = string.Empty;
    public long? Ci { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Celular { get; set; }
    public string? Telefono { get; set; }
    public DateOnly FechaNacimiento { get; set; }

    // Seguridad e Intentos
    public string PasswordHash { get; set; } = string.Empty;
    public int IntentosFallidos { get; set; } = 0;

    // Estados
    public bool Habilitado { get; set; } = true;
    public bool ExpiradoPorInactividad { get; set; } = false;

    // Fechas
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? UltimoAcceso { get; set; }

    // Claves Foráneas (FKs)
    public int IdNacionalidad { get; set; }
    public Nacionalidad Nacionalidad { get; set; } = null!;

    public int? IdEstadoCivil { get; set; }
    public EstadoCivil? EstadoCivil { get; set; }

    public int? IdProfesion { get; set; }
    public Profesion? Profesion { get; set; }

    public int? IdGrado { get; set; }
    public Grado? Grado { get; set; }

    public int? IdEscalafon { get; set; }
    public Escalafon? Escalafon { get; set; }

    public int? IdDependencia { get; set; }
    public Dependencia? Dependencia { get; set; }

    // Colecciones / Relaciones
    public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    public ICollection<UsuarioPermiso> UsuarioPermisos { get; set; } = new List<UsuarioPermiso>();
    public ICollection<HistorialUsuario> Historiales { get; set; } = new List<HistorialUsuario>();
    public ICollection<HistorialPassword> HistorialPasswords { get; set; } = new List<HistorialPassword>();
}

public class HistorialUsuario
{
    public long IdHistorial { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string TipoAccion { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public string RealizadoPor { get; set; } = string.Empty;

    public string UsuarioNombreUsuario { get; set; } = string.Empty;
    public Usuario Usuario { get; set; } = null!;
}

public class HistorialPassword
{
    public long IdHistorialPassword { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public string UsuarioNombreUsuario { get; set; } = string.Empty;
    public Usuario Usuario { get; set; } = null!;
}