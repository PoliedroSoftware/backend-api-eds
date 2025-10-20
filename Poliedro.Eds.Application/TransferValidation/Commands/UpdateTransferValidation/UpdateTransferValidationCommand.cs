using MediatR;
using Poliedro.Eds.Application.TransferValidation.Dtos;

namespace Poliedro.Eds.Application.TransferValidation.Commands.UpdateTransferValidation;

public record UpdateTransferValidationCommand(
    int Id,
    UpdateTransferValidationRequestDto Request) : IRequest<TransferValidationDto>;
