using MediatR;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Events;
using Poliedro.Eds.Domain.Common.Events.Cache;

namespace Poliedro.Eds.Application.Common.EventHandlers.Cache;


public class EntityModifiedEventHandler : IDomainEventHandler<EntityModifiedDomainEvent>
{
    private readonly IRedisService _redisService;
    private readonly ILogger<EntityModifiedEventHandler> _logger;
    
    private static readonly Dictionary<string, string[]> EntityCacheRelations = new()
    {
        ["court"] = new[] { "court", "business", "dispensers", "product", "inventory", "expenditures", "typeofcollection" },
        ["business"] = new[] { "business", "eds", "island" },
        ["product"] = new[] { "product", "inventory", "compartiment", "productcompartment" },
        ["islander"] = new[] { "islander", "business" },
        ["dispensers"] = new[] { "dispensers", "hose", "island" },
        ["tank"] = new[] { "tank", "edstank", "compartiment" },
        ["inventory"] = new[] { "inventory", "product", "shoppingproductinventory" },
        ["expenditures"] = new[] { "expenditures" },
        ["typeofcollection"] = new[] { "typeofcollection" },
        ["compartiment"] = new[] { "compartiment", "tank", "product" },
        ["hose"] = new[] { "hose", "dispensers", "hosehistory" },
        ["island"] = new[] { "island", "dispensers", "business" },
        ["shopping"] = new[] { "shopping", "shoppingproduct", "inventory", "product" }
    };

    public EntityModifiedEventHandler(
        IRedisService redisService,
        ILogger<EntityModifiedEventHandler> logger)
    {
        _redisService = redisService;
        _logger = logger;
    }

    public async Task HandleAsync(EntityModifiedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("🔄 Procesando modificación de entidad '{EntityType}' con operación '{Operation}' para tenant '{Tenant}'",
                domainEvent.EntityType, domainEvent.Operation, domainEvent.Tenant);

            var relatedEntities = GetRelatedEntitiesForCache(domainEvent.EntityType);
            
            var tags = new List<string>();
            
            foreach (var entity in relatedEntities)
            {
                tags.Add(_redisService.GetCacheTag(domainEvent.Tenant, entity));
                tags.Add(_redisService.GetCacheTag(domainEvent.Tenant, $"{entity}:list"));
            }
            
            tags.Add(_redisService.GetCacheTag(domainEvent.Tenant, $"{domainEvent.EntityType}:{domainEvent.EntityId}"));

            await _redisService.InvalidateCacheByTagsAsync(tags.ToArray());
            
            await _redisService.PublishCacheInvalidationAsync(domainEvent.Tenant, tags.ToArray());

            _logger.LogInformation("✅ Invalidación de caché completada para {EntityCount} tipos de entidad relacionados",
                relatedEntities.Length);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error procesando evento de modificación de entidad '{EntityType}' para tenant '{Tenant}'",
                domainEvent.EntityType, domainEvent.Tenant);
        }
    }

    private static string[] GetRelatedEntitiesForCache(string entityType)
    {
        var normalizedType = entityType.ToLowerInvariant();
        
        if (EntityCacheRelations.TryGetValue(normalizedType, out var relatedEntities))
        {
            return relatedEntities;
        }

        return new[] { normalizedType };
    }
}
