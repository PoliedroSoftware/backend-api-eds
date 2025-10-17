using MediatR;
using Poliedro.Eds.Application.TransferValidation.Dtos;

namespace Poliedro.Eds.Application.TransferValidation.Commands.CreateTransferValidation;

public record CreateTransferValidationCommand(
    TransferValidationCreateRequestDto Request) : IRequest<TransferValidationDto>;
