using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.Common.Services.Background;


public class CacheInvalidationSubscriberService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CacheInvalidationSubscriberService> _logger;

    public CacheInvalidationSubscriberService(
        IServiceProvider serviceProvider,
        ILogger<CacheInvalidationSubscriberService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🔔 Iniciando servicio de suscripción a invalidación de caché...");

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var redisService = scope.ServiceProvider.GetRequiredService<IRedisService>();

            await redisService.SubscribeToCacheInvalidationAsync(OnCacheInvalidationReceived);

            _logger.LogInformation("✅ Servicio de suscripción a invalidación de caché iniciado correctamente");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("🛑 Servicio de suscripción a invalidación de caché detenido");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error en servicio de suscripción a invalidación de caché");
        }
    }

    private async Task OnCacheInvalidationReceived(string tenant, string[] tags)
    {
        try
        {
            _logger.LogInformation("📨 Recibida notificación de invalidación de caché para tenant '{Tenant}' con tags: {Tags}",
                tenant, string.Join(", ", tags));

            using var scope = _serviceProvider.CreateScope();
            var redisService = scope.ServiceProvider.GetRequiredService<IRedisService>();

            // Invalidar caché local basado en los tags recibidos
            await redisService.InvalidateCacheByTagsAsync(tags);

            _logger.LogInformation("✅ Caché invalidado exitosamente para tenant '{Tenant}'", tenant);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error procesando notificación de invalidación de caché para tenant '{Tenant}'", tenant);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🛑 Deteniendo servicio de suscripción a invalidación de caché...");
        await base.StopAsync(stoppingToken);
    }
}
