using Poliedro.Eds.Domain.IoT.Models;

namespace Poliedro.Eds.Domain.IoT.Ports;

public interface IIoTPublishService
{
    Task<bool> PublishMessageAsync(IoTMessage message, string topic);
}
