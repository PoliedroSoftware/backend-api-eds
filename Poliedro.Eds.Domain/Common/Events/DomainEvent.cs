namespace Poliedro.Eds.Domain.Common.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
    Guid EventId { get; }
}

public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; private set; }
    public Guid EventId { get; private set; }

    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}
