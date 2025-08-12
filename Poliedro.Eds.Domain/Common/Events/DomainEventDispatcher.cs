using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.Common.Events;

namespace Poliedro.Eds.Domain.Common.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAsync<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken = default) 
        where TDomainEvent : IDomainEvent;
    
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(IServiceProvider serviceProvider, ILogger<DomainEventDispatcher> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task DispatchAsync<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken = default) 
        where TDomainEvent : IDomainEvent
    {
        _logger.LogInformation("Despachando evento de dominio: {EventType} con ID: {EventId}", 
            typeof(TDomainEvent).Name, domainEvent.EventId);

        var handlers = _serviceProvider.GetServices<IDomainEventHandler<TDomainEvent>>();
        
        var tasks = handlers.Select(handler => 
            HandleSafely(() => handler.HandleAsync(domainEvent, cancellationToken), handler.GetType().Name));
        
        await Task.WhenAll(tasks);
        
        _logger.LogInformation("Evento de dominio {EventType} procesado por {HandlerCount} handlers", 
            typeof(TDomainEvent).Name, handlers.Count());
    }

    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            await DispatchDynamically(domainEvent, cancellationToken);
        }
    }

    private async Task DispatchDynamically(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var eventType = domainEvent.GetType();
        var method = typeof(DomainEventDispatcher)
            .GetMethod(nameof(DispatchAsync), new[] { eventType, typeof(CancellationToken) })
            ?.MakeGenericMethod(eventType);

        if (method != null)
        {
            var task = (Task?)method.Invoke(this, new object[] { domainEvent, cancellationToken });
            if (task != null)
            {
                await task;
            }
        }
    }

    private async Task HandleSafely(Func<Task> handler, string handlerName)
    {
        try
        {
            await handler();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar evento de dominio en handler: {HandlerName}", handlerName);
         
        }
    }
}
