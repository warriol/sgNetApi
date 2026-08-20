namespace sgNetApi.Domain.Entities;

public class Usuario
{
    public long Ci { get; set; } // PK: Cédula de Identidad
    public string NombreUsuario { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public long Celular { get; set; }

    // Seguridad e Intentos
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    public int IntentosFallidos { get; set; } = 0;

    // Estados
    public bool Habilitado { get; set; } = true;
    public bool ExpiradoPorInactividad { get; set; } = false;

    // Fechas
    public DateTime Creado { get; set; } = DateTime.UtcNow;
    public DateTime? UltimoAcceso { get; set; }

    // Claves Foráneas (FKs)
    public int IdGrado { get; set; }
    public Grado Grado { get; set; } = null!;

    public int IdEscalafon { get; set; }
    public Escalafon Escalafon { get; set; } = null!;

    public int IdUuee { get; set; }
    public UnidadEjecutora UnidadEjecutora { get; set; } = null!;

    public int IdDependencia { get; set; }
    public Dependencia Dependencia { get; set; } = null!;

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

    public long UsuarioCi { get; set; }
    public Usuario Usuario { get; set; } = null!;
}

public class HistorialPassword
{
    public long IdHistorialPassword { get; set; }
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public long UsuarioCi { get; set; }
    public Usuario Usuario { get; set; } = null!;
}