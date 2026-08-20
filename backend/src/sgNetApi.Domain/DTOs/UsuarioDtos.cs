namespace sgNetApi.Domain.DTOs;

public class UsuarioDetalleDto
{
    public long Ci { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public long Celular { get; set; }
    public bool Habilitado { get; set; }
    public bool ExpiradoPorInactividad { get; set; }
    public DateTime Creado { get; set; }
    public DateTime? UltimoAcceso { get; set; }
    
    // Nombres legibles de las dependencias
    public string Grado { get; set; } = string.Empty;
    public string Escalafon { get; set; } = string.Empty;
    public string UnidadEjecutora { get; set; } = string.Empty;
    public string Dependencia { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = new();
    public List<string> PermisosDirectos { get; set; } = new();
}

public class CrearUsuarioDto
{
    public long Ci { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public long Celular { get; set; }
    public string Password { get; set; } = string.Empty; // Si viene vacío se usa la CI por defecto

    public int IdGrado { get; set; }
    public int IdEscalafon { get; set; }
    public int IdUuee { get; set; }
    public int IdDependencia { get; set; }

    public List<int> IdsRoles { get; set; } = new();
    public List<int> IdsPermisosDirectos { get; set; } = new();
}

public class EditarUsuarioDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public long Celular { get; set; }

    public int IdGrado { get; set; }
    public int IdEscalafon { get; set; }
    public int IdUuee { get; set; }
    public int IdDependencia { get; set; }

    public List<int> IdsRoles { get; set; } = new();
    public List<int> IdsPermisosDirectos { get; set; } = new();
}

public class CambiarEstadoUsuarioDto
{
    public bool Habilitado { get; set; }
    public string Observacion { get; set; } = string.Empty;
}