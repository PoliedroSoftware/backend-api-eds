namespace Poliedro.Eds.Domain.Business.Events;

public static class BusinessEvents
{
    public static readonly DomainEvent<BusinessCreated> BusinessCreatedEvents = new DomainEvent<BusinessCreated>();
}
