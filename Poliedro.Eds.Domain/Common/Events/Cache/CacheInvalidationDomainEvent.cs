using Poliedro.Eds.Domain.Common.Events;

namespace Poliedro.Eds.Domain.Common.Events.Cache;

/// <summary>
/// Evento de dominio para invalidación de caché
/// </summary>
public class CacheInvalidationDomainEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string Tenant { get; }
    public string Operation { get; }
    public string EntityType { get; }
    public object? EntityId { get; }
    public string[] AdditionalTags { get; }

    public CacheInvalidationDomainEvent(
        string tenant,
        string operation,
        string entityType,
        object? entityId = null,
        params string[] additionalTags)
    {
        Tenant = tenant;
        Operation = operation;
        EntityType = entityType;
        EntityId = entityId;
        AdditionalTags = additionalTags ?? Array.Empty<string>();
    }
}
