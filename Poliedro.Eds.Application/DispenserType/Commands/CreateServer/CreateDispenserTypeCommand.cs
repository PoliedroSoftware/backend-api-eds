using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;


namespace Poliedro.Eds.Application.DispenserType.Commands.CreateDispenserType;

public record CreateDispenserTypeCommand(CreateDispenserTypeRequestDto Request) : IRequest<Result<VoidResult, Error>>;


