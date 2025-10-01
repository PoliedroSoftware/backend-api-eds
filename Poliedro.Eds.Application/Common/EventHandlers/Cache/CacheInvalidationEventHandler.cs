using MediatR;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Events;
using Poliedro.Eds.Domain.Common.Events.Cache;

namespace Poliedro.Eds.Application.Common.EventHandlers.Cache;

/// <summary>
/// Handler para eventos de invalidación de caché
/// </summary>
public class CacheInvalidationEventHandler : IDomainEventHandler<CacheInvalidationDomainEvent>
{
    private readonly IRedisService _redisService;
    private readonly ILogger<CacheInvalidationEventHandler> _logger;

    public CacheInvalidationEventHandler(
        IRedisService redisService,
        ILogger<CacheInvalidationEventHandler> logger)
    {
        _redisService = redisService;
        _logger = logger;
    }

    public async Task HandleAsync(CacheInvalidationDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("🗑️ Procesando invalidación de caché para tenant '{Tenant}', operación '{Operation}', entidad '{EntityType}'",
                domainEvent.Tenant, domainEvent.Operation, domainEvent.EntityType);

            await _redisService.InvalidateDistributedCacheAsync(
                domainEvent.Tenant,
                domainEvent.Operation,
                domainEvent.EntityType,
                domainEvent.EntityId);

            _logger.LogInformation("✅ Invalidación de caché completada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error procesando evento de invalidación de caché para tenant '{Tenant}', entidad '{EntityType}'",
                domainEvent.Tenant, domainEvent.EntityType);
        }
    }
}
