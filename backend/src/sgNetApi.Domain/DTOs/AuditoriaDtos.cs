namespace sgNetApi.Domain.DTOs;

public class FiltroAuditoriaDto
{
    public string? UsuarioCi { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public int? CodigoEstado { get; set; }

    // Parámetros de Paginación
    public int Pagina { get; set; } = 1;
    public int RegistrosPorPagina { get; set; } = 10;
}

public class AuditoriaLogDto
{
    public long Id { get; set; }
    public DateTime Fecha { get; set; }
    public string UsuarioCi { get; set; } = string.Empty;
    public string IpOrigen { get; set; } = string.Empty;
    public string MetodoHttp { get; set; } = string.Empty;
    public string Ruta { get; set; } = string.Empty;
    public int CodigoEstado { get; set; }
    public long TiempoEjecucionMs { get; set; }
    public string? Excepcion { get; set; }
}

public class ResultadoPaginadoDto<T>
{
    public List<T> Elementos { get; set; } = new();
    public int PaginaActual { get; set; }
    public int TotalPaginas { get; set; }
    public int TotalRegistros { get; set; }
    public bool TienePaginaAnterior => PaginaActual > 1;
    public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
}