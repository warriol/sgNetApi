namespace sgNetApi.Domain.DTOs;

public class PermisoDto
{
    public int IdPermiso { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}

public class RolDetalleDto
{
    public int IdRol { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public List<PermisoDto> Permisos { get; set; } = new();
}

public class CrearRolDto
{
    public string Nombre { get; set; } = string.Empty;
    public List<int> IdsPermisos { get; set; } = new();
}

public class EditarRolDto
{
    public string Nombre { get; set; } = string.Empty;
    public List<int> IdsPermisos { get; set; } = new();
}

public class AsignarRolesPermisosUsuarioDto
{
    public List<int> IdsRoles { get; set; } = new();
    public List<int> IdsPermisosDirectos { get; set; } = new();
}