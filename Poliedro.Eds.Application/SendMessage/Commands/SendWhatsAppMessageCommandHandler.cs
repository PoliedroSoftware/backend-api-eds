using MediatR;
using Poliedro.Eds.Domain.SendMessage;

public class SendWhatsAppMessageCommandHandler (ISendMessage sendMessage) : IRequestHandler<SendWhatsAppMessageCommand, Unit>
{
    public async Task<Unit> Handle(SendWhatsAppMessageCommand request, CancellationToken cancellationToken)
    {
        await sendMessage.SendMessageAsync(request.PhoneNumber, request.Message);
        return Unit.Value;
    }
}
