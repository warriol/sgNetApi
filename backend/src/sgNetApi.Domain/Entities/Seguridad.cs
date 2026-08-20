namespace sgNetApi.Domain.Entities;

public class Permiso
{
    public int IdPermiso { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
    public ICollection<UsuarioPermiso> UsuarioPermisos { get; set; } = new List<UsuarioPermiso>();
}

public class Rol
{
    public int IdRol { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
    public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
}

public class RolPermiso
{
    public int IdRol { get; set; }
    public Rol Rol { get; set; } = null!;

    public int IdPermiso { get; set; }
    public Permiso Permiso { get; set; } = null!;
}

public class UsuarioRol
{
    public long UsuarioCi { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int IdRol { get; set; }
    public Rol Rol { get; set; } = null!;
}

public class UsuarioPermiso
{
    public long UsuarioCi { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int IdPermiso { get; set; }
    public Permiso Permiso { get; set; } = null!;
}