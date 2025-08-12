using Microsoft.Extensions.DependencyInjection;
using Poliedro.Eds.Domain.Business.Events;
using Poliedro.Eds.Domain.Business.EventHandlers;
using Poliedro.Eds.Domain.Common.Events;

namespace Poliedro.Eds.Domain.Business.Extensions;

public static class DomainServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessDomainEvents(this IServiceCollection services)
    {
        // Registrar el dispatcher de eventos
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        
        // Registrar los handlers de eventos
        services.AddScoped<IDomainEventHandler<BusinessCreated>, BusinessCreatedEventHandler>();
        
        return services;
    }
}
