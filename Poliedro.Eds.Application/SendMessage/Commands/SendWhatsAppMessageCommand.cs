using MediatR;

public class SendWhatsAppMessageCommand : IRequest<Unit>
{
    public string PhoneNumber { get; set; }
    public string Message { get; set; }

    public SendWhatsAppMessageCommand(string phoneNumber, string message)
    {
        PhoneNumber = phoneNumber;
        Message = message;
    }
}
