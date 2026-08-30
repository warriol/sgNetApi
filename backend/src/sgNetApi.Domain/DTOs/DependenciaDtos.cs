namespace sgNetApi.Domain.DTOs;

public class DependenciaDto
{
    public int IdDependencia { get; set; }
    public int IdUuee { get; set; }
    public string NombreUuee { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Siglas { get; set; } = string.Empty;
    public int? IdDireccion { get; set; }
    public string? NombreUsuarioJefe { get; set; }
    public string? NombreUsuarioSegundoJefe { get; set; }
    public int? IdTurno { get; set; }
    public string? NombreTurno { get; set; }
    public int FuncionariosAsignados { get; set; }
    public bool Completada { get; set; }
}

public class CrearDependenciaDto
{
    public int? IdUuee { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Siglas { get; set; } = string.Empty;
    public int? IdDireccion { get; set; }
    public string? NombreUsuarioJefe { get; set; }
    public string? NombreUsuarioSegundoJefe { get; set; }
    public int? IdTurno { get; set; }
}

public class ReemplazarFuncionariosDependenciaDto
{
    public List<string> NombresUsuario { get; set; } = new();
}

public class JefaturasDependenciaDto
{
    public string? NombreUsuarioJefe { get; set; }
    public string? NombreUsuarioSegundoJefe { get; set; }
}

public class TurnoDependenciaDto
{
    public int IdTurno { get; set; }
}

public class DependenciaCompletitudDto
{
    public bool TieneNombreYSiglas { get; set; }
    public bool TieneUnidadEjecutora { get; set; }
    public bool TieneAlMenosDosFuncionarios { get; set; }
    public bool JefesValidos { get; set; }
    public bool TieneDireccion { get; set; }
    public bool TieneTurnoHabilitado { get; set; }
    public bool Completada { get; set; }
    public List<string> Pendientes { get; set; } = new();
}
