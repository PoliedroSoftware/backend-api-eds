using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.Common.Events;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Events;

namespace Poliedro.Eds.Application.Islander.EventHandlers;

public class IslanderKeycloakCreatedEventHandler : INotificationHandler<IslanderKeycloakCreatedEvent>
{
    private readonly IIslanderCreateIslander _islanderCreateService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<IslanderKeycloakCreatedEventHandler> _logger;

    public IslanderKeycloakCreatedEventHandler(
        IIslanderCreateIslander islanderCreateService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<IslanderKeycloakCreatedEventHandler> logger)
    {
        _islanderCreateService = islanderCreateService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task Handle(IslanderKeycloakCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Procesando evento IslanderKeycloakCreatedEvent para Islander ID: {IslanderId}, Tenant: {Tenant}", 
                notification.Islander.IdEds, notification.Tenant);
            var result = await _islanderCreateService.CreateAsync(notification.Islander, notification.Tenant);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Islander creado exitosamente en la base de datos. ID: {IslanderId}, Tenant: {Tenant}", 
                    notification.Islander.IdEds, notification.Tenant);
            }
            else
            {
                _logger.LogError("Error al crear Islander en la base de datos. ID: {IslanderId}, Error: {Error}", 
                    notification.Islander.IdEds, result.Error?.Description);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al procesar IslanderKeycloakCreatedEvent para Islander ID: {IslanderId}", 
                notification.Islander.IdEds);
        }
    }
}
