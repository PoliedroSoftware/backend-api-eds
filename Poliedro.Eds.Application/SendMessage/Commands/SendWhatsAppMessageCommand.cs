using MediatR;
using Poliedro.Eds.Application.Court.Dtos;

public class SendWhatsAppMessageCommand : IRequest<Unit>
{
    public string PhoneNumber { get; init; }
    public CourtDto Court { get; init; }

}
