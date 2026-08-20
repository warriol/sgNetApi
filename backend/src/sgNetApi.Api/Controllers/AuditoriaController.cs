using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sgNetApi.Api.Authorization;
using sgNetApi.Domain.DTOs;
using sgNetApi.Infrastructure.Data;

namespace sgNetApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditoriaController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuditoriaController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Consulta paginada del historial de auditoría HTTP y logs del sistema.
    /// </summary>
    [HttpGet]
    [RequirePermission("admin.auditoria.leer")]
    public async Task<IActionResult> ObtenerTodos([FromQuery] FiltroAuditoriaDto filtro)
    {
        // Limitar tamaño máximo de página por seguridad
        int limitePorPagina = filtro.RegistrosPorPagina > 100 ? 100 : filtro.RegistrosPorPagina;
        if (filtro.Pagina < 1) filtro.Pagina = 1;

        var query = _context.AuditoriaLogs.AsNoTracking().AsQueryable();

        // Aplicar Filtros Opcionales
        if (!string.IsNullOrWhiteSpace(filtro.UsuarioCi))
        {
            query = query.Where(a => a.UsuarioCi.Contains(filtro.UsuarioCi));
        }

        if (filtro.FechaDesde.HasValue)
        {
            query = query.Where(a => a.Fecha >= filtro.FechaDesde.Value.ToUniversalTime());
        }

        if (filtro.FechaHasta.HasValue)
        {
            query = query.Where(a => a.Fecha <= filtro.FechaHasta.Value.ToUniversalTime());
        }

        if (filtro.CodigoEstado.HasValue)
        {
            query = query.Where(a => a.CodigoEstado == filtro.CodigoEstado.Value);
        }

        int totalRegistros = await query.CountAsync();
        int totalPaginas = (int)Math.Ceiling(totalRegistros / (double)limitePorPagina);

        var logs = await query
            .OrderByDescending(a => a.Fecha)
            .Skip((filtro.Pagina - 1) * limitePorPagina)
            .Take(limitePorPagina)
            .Select(a => new AuditoriaLogDto
            {
                Id = a.Id,
                Fecha = a.Fecha,
                UsuarioCi = a.UsuarioCi,
                IpOrigen = a.IpOrigen,
                MetodoHttp = a.MetodoHttp,
                Ruta = a.Ruta,
                CodigoEstado = a.CodigoEstado,
                TiempoEjecucionMs = a.TiempoEjecucionMs,
                Excepcion = a.Excepcion
            })
            .ToListAsync();

        var resultado = new ResultadoPaginadoDto<AuditoriaLogDto>
        {
            Elementos = logs,
            PaginaActual = filtro.Pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    }

    /// <summary>
    /// Exporta los logs de auditoría filtrados directamente a un archivo descargable en formato CSV de forma eficiente.
    /// </summary>
    [HttpGet("exportar")]
    [RequirePermission("admin.auditoria.exportar")]
    public async Task<IActionResult> ExportarACsv([FromQuery] FiltroAuditoriaDto filtro)
    {
        var query = _context.AuditoriaLogs.AsNoTracking().AsQueryable();

        // Aplicamos los mismos filtros de búsqueda que el listado general
        if (!string.IsNullOrWhiteSpace(filtro.UsuarioCi))
        {
            query = query.Where(a => a.UsuarioCi.Contains(filtro.UsuarioCi));
        }
        if (filtro.FechaDesde.HasValue)
        {
            query = query.Where(a => a.Fecha >= filtro.FechaDesde.Value.ToUniversalTime());
        }
        if (filtro.FechaHasta.HasValue)
        {
            query = query.Where(a => a.Fecha <= filtro.FechaHasta.Value.ToUniversalTime());
        }
        if (filtro.CodigoEstado.HasValue)
        {
            query = query.Where(a => a.CodigoEstado == filtro.CodigoEstado.Value);
        }

        // Traemos los datos ordenados cronológicamente de forma descendente
        var logs = await query.OrderByDescending(a => a.Fecha).ToListAsync();

        var csvBuilder = new StringBuilder();
        
        // Escribimos los encabezados de las columnas del reporte
        csvBuilder.AppendLine("Id,Fecha (UTC),Usuario CI,IP Origen,Metodo HTTP,Ruta,Codigo Estado,Tiempo Ejecucion (ms),Excepcion / Detalles");

        foreach (var log in logs)
        {
            // Sanitizar campos de texto para evitar inyecciones CSV o roturas de filas por comas internas
            string rutaSanitizada = log.Ruta.Contains(",") ? $"\"{log.Ruta}\"" : log.Ruta;
            string excepcionSanitizada = string.IsNullOrEmpty(log.Excepcion) 
                ? "" 
                : $"\"{log.Excepcion.Replace("\"", "\"\"").Replace("\r\n", " ").Replace("\n", " ")}\"";

            csvBuilder.AppendLine($"{log.Id},{log.Fecha:yyyy-MM-dd HH:mm:ss},{log.UsuarioCi},{log.IpOrigen},{log.MetodoHttp},{rutaSanitizada},{log.CodigoEstado},{log.TiempoEjecucionMs},{excepcionSanitizada}");
        }

        // UTF-8 con BOM (Byte Order Mark) para que Microsoft Excel reconozca automáticamente las tildes y caracteres especiales en español
        var bytesConBom = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csvBuilder.ToString())).ToArray();

        string nombreArchivo = $"Auditoria_Logs_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
        
        return File(bytesConBom, "text/csv", nombreArchivo);
    }
}