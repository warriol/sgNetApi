using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using sgNetApi.Infrastructure.Data;

namespace sgNetApi.Infrastructure.Workers;

public class AuditoriaCleanupWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AuditoriaCleanupWorker> _logger;
    private readonly TimeSpan _intervaloEjecucion = TimeSpan.FromHours(24); // Se ejecuta una vez al día
    private const int DiasRetencion = 365; // Retención de 1 año

    public AuditoriaCleanupWorker(IServiceProvider serviceProvider, ILogger<AuditoriaCleanupWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servicio de limpieza automática de Auditoría iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await EjecutarDepuracionAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error no controlado durante la depuración de logs de auditoría.");
            }

            // Espera 24 horas antes de la siguiente ejecución (respetando la cancelación de la App)
            await Task.Delay(_intervaloEjecucion, stoppingToken);
        }
    }

    private async Task EjecutarDepuracionAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando proceso de purga de logs de auditoría con más de {Dias} días...", DiasRetencion);

        // Crear un Scope explícito porque AppDbContext es un servicio Scoped y BackgroundService es Singleton
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var fechaLímite = DateTime.UtcNow.AddDays(-DiasRetencion);

        // En EF Core 7+, ExecuteDeleteAsync realiza un DELETE directo en PostgreSQL sin cargar las entidades a memoria
        int registrosEliminados = await context.AuditoriaLogs
            .Where(a => a.Fecha < fechaLímite)
            .ExecuteDeleteAsync(cancellationToken);

        if (registrosEliminados > 0)
        {
            _logger.LogInformation("Depuración completada exitosamente. Se eliminaron {Cantidad} registros anteriores a {Fecha}.", 
                registrosEliminados, fechaLímite.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        else
        {
            _logger.LogInformation("No se encontraron registros de auditoría antiguos para purgar.");
        }
    }
}