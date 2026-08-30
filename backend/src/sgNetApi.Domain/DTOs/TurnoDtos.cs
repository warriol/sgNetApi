namespace sgNetApi.Domain.DTOs;

public class TurnoDto
{
    public int IdTurno { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string TipoTurno { get; set; } = "4x6";
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly? HoraFin { get; set; }
    public string? Descripcion { get; set; }
    public bool Habilitado { get; set; }
}

public class CrearTurnoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string TipoTurno { get; set; } = "4x6";
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly? HoraFin { get; set; }
    public string? Descripcion { get; set; }
    public bool Habilitado { get; set; } = true;
}
