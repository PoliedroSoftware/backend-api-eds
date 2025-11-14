using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.IoT.Ports;

namespace Poliedro.Eds.Application.IoT.Commands.PublishMessage;

public class PublishIoTMessageCommandHandler(
    IIoTPublishService ioTPublishService,
    IConfiguration configuration,
    ILogger<PublishIoTMessageCommandHandler> logger) 
    : IRequestHandler<PublishIoTMessageCommand, bool>
{
    public async Task<bool> Handle(PublishIoTMessageCommand request, CancellationToken cancellationToken)
    {
        var topic = request.Topic ?? configuration["AWS:IoT:DefaultTopic"];

        if (string.IsNullOrWhiteSpace(topic))
        {
            logger.LogError("No topic provided and no default topic configured in AWS:IoT:DefaultTopic");
            return false;
        }

        logger.LogInformation("Publishing IoT message to topic: {Topic}", topic);

        try
        {
            var result = await ioTPublishService.PublishMessageAsync(request.Message, topic);
            
            if (result)
            {
                logger.LogInformation("IoT message published successfully to topic: {Topic}", topic);
            }
            else
            {
                logger.LogWarning("Failed to publish IoT message to topic: {Topic}", topic);
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred while publishing IoT message to topic: {Topic}", topic);
            return false;
        }
    }
}
