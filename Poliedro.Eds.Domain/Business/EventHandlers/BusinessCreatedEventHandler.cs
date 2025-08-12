using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.Business.Events;
using Poliedro.Eds.Domain.Common.Events;

namespace Poliedro.Eds.Domain.Business.EventHandlers;

public class BusinessCreatedEventHandler(ILogger<BusinessCreatedEventHandler> _logger) : IDomainEventHandler<BusinessCreated>
{
    public async Task HandleAsync(BusinessCreated domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Manejando evento BusinessCreated para Business: {BusinessId}, Nombre: {Name}, Contexto: {Context}", 
            domainEvent.BusinessId, domainEvent.Name, domainEvent.Context);

        // Aquí puedes agregar lógica adicional como:
        // - Enviar notificaciones
        // - Crear registros de auditoría
        // - Integrar con sistemas externos
        // - Actualizar caches
        
       
        _logger.LogInformation("Evento BusinessCreated procesado exitosamente para Business: {BusinessId}", 
            domainEvent.BusinessId);
    }
}
