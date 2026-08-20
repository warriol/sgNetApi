using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using sgNetApi.Domain.Entities;
using sgNetApi.Infrastructure.Data;

namespace sgNetApi.Api.Middlewares;

public class AuditAndExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuditAndExceptionMiddleware> _logger;

    public AuditAndExceptionMiddleware(RequestDelegate next, ILogger<AuditAndExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        var cronometro = Stopwatch.StartNew();
        string? excepcionTexto = null;

        try
        {
            // Continuar con el pipeline de la petición
            await _next(context);
        }
        catch (Exception ex)
        {
            cronometro.Stop();
            excepcionTexto = ex.ToString();
            _logger.LogError(ex, "Excepción no controlada en {Path}", context.Request.Path);

            // Manejar la respuesta de error de forma limpia para el cliente
            await ManejarExcepcionAsync(context, ex);
        }
        finally
        {
            cronometro.Stop();

            // Omitir el registro automático de llamadas a Swagger UI para no saturar la BD
            if (!context.Request.Path.StartsWithSegments("/swagger"))
            {
                await RegistrarAuditoriaAsync(context, serviceProvider, cronometro.ElapsedMilliseconds, excepcionTexto);
            }
        }
    }

    private static async Task ManejarExcepcionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var respuesta = new
        {
            codigo = context.Response.StatusCode,
            mensaje = "Ocurrió un error interno en el servidor. El incidente ha sido registrado.",
            fecha = DateTime.UtcNow
        };

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(respuesta, jsonOptions));
    }

    private static async Task RegistrarAuditoriaAsync(
        HttpContext context, 
        IServiceProvider serviceProvider, 
        long tiempoMs, 
        string? excepcionTexto)
    {
        try
        {
            // Crear un Scope para obtener el AppDbContext Scoped dentro del Middleware Singleton/Transient
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            string usuarioCi = context.User.FindFirstValue(ClaimTypes.NameIdentifier) 
                               ?? context.User.FindFirstValue("ci") 
                               ?? "ANONIMO";

            string ipOrigen = context.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";

            var log = new AuditoriaLog
            {
                Fecha = DateTime.UtcNow,
                UsuarioCi = usuarioCi,
                IpOrigen = ipOrigen,
                MetodoHttp = context.Request.Method,
                Ruta = context.Request.Path,
                CodigoEstado = context.Response.StatusCode,
                TiempoEjecucionMs = tiempoMs,
                Excepcion = excepcionTexto
            };

            dbContext.AuditoriaLogs.Add(log);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Evitar que un fallo al escribir el log rompa la respuesta del cliente
            Debug.WriteLine($"Error al guardar AuditoriaLog: {ex.Message}");
        }
    }
}