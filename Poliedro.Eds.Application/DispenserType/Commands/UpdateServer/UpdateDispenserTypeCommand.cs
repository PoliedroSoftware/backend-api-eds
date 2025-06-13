using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.DispenserType.Commands.UpdateDispenserType;

    public record UpdateDispenserTypeCommand(
    int IdType,
    string Description) : IRequest<Result<VoidResult, Error>>;