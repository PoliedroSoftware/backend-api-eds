using MediatR;
using Poliedro.Eds.Application.Court.Dtos;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Domain.Hose.Dtos;

public class SendWhatsAppMessageCommand : IRequest<Unit>
{
    public string PhoneNumber { get; init; }

    public CourtDto Court { get; init; }


}
