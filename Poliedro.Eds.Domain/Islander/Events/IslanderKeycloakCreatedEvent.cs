using MediatR;
using Poliedro.Eds.Domain.Common.Events;
using Poliedro.Eds.Domain.Islander.Entities;

namespace Poliedro.Eds.Domain.Islander.Events;

public class IslanderKeycloakCreatedEvent : DomainEvent, INotification
{
    public IslanderEntity Islander { get; }
    public string Tenant { get; }

    public IslanderKeycloakCreatedEvent(IslanderEntity islander, string tenant)
    {
        Islander = islander;
        Tenant = tenant;
    }
}
