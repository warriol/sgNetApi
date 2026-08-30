namespace sgNetApi.Domain.DTOs;

public class FuncionarioEquipoPolicialDto
{
    public int IdFuncionarioEquipoPolicial { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public int IdArmaPolicial { get; set; }
    public int IdChalecoAntibalaPolicial { get; set; }
    public int IdEsposasPolicial { get; set; }
    public DateTime FechaAsignacion { get; set; }
    public DateTime? FechaDevolucion { get; set; }
    public string? Observaciones { get; set; }
}

public class CrearFuncionarioEquipoPolicialDto
{
    public string NombreUsuario { get; set; } = string.Empty;
    public int IdArmaPolicial { get; set; }
    public int IdChalecoAntibalaPolicial { get; set; }
    public int IdEsposasPolicial { get; set; }
    public string? Observaciones { get; set; }
}

public class DevolucionEquipoDto
{
    public DateTime? FechaDevolucion { get; set; }
    public string? Observaciones { get; set; }
}
