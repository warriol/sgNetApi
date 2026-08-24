namespace sgNetApi.Domain.Entities;

public class AuditoriaLog
{
    public long IdLog { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string? UsuarioNombreUsuario { get; set; }
    public string IpOrigen { get; set; } = string.Empty;
    public string MetodoHttp { get; set; } = string.Empty;
    public string Ruta { get; set; } = string.Empty;
    public int CodigoEstado { get; set; }
    public long DuracionMs { get; set; }
    public string? PayloadRequest { get; set; }
}