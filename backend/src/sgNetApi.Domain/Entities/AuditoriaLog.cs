namespace sgNetApi.Domain.Entities;

public class AuditoriaLog
{
    public long Id { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string UsuarioCi { get; set; } = "ANONIMO";
    public string IpOrigen { get; set; } = string.Empty;
    public string MetodoHttp { get; set; } = string.Empty;
    public string Ruta { get; set; } = string.Empty;
    public int CodigoEstado { get; set; }
    public long TiempoEjecucionMs { get; set; }
    public string? Excepcion { get; set; }
}