using MediatR;

public class SendWhatsAppMessageCommandHandler : IRequestHandler<SendWhatsAppMessageCommand, Unit>
{
    private readonly IWhatsAppService _whatsAppService;

    public SendWhatsAppMessageCommandHandler(IWhatsAppService whatsAppService)
    {
        _whatsAppService = whatsAppService;
    }

    public async Task<Unit> Handle(SendWhatsAppMessageCommand request, CancellationToken cancellationToken)
    {
        await _whatsAppService.SendMessageAsync(request.PhoneNumber, request.Message);
        return Unit.Value;
    }
}
