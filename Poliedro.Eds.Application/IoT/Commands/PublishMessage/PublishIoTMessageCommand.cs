using MediatR;
using Poliedro.Eds.Domain.IoT.Models;

namespace Poliedro.Eds.Application.IoT.Commands.PublishMessage;

public record PublishIoTMessageCommand(IoTMessage Message, string? Topic) : IRequest<bool>;
