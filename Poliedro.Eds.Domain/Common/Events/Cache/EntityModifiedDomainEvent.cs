using Poliedro.Eds.Domain.Common.Events;

namespace Poliedro.Eds.Domain.Common.Events.Cache;

/// <summary>
/// Evento de dominio específico para operaciones de escritura que requieren invalidación de caché
/// </summary>
public class EntityModifiedDomainEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string Tenant { get; }
    public string EntityType { get; }
    public object EntityId { get; }
    public EntityOperation Operation { get; }
    public object? EntityData { get; }

    public EntityModifiedDomainEvent(
        string tenant,
        string entityType,
        object entityId,
        EntityOperation operation,
        object? entityData = null)
    {
        Tenant = tenant;
        EntityType = entityType;
        EntityId = entityId;
        Operation = operation;
        EntityData = entityData;
    }
}

public enum EntityOperation
{
    Created,
    Updated,
    Deleted
}
