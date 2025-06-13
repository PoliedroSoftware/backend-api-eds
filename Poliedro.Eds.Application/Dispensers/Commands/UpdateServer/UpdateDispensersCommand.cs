using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Dispensers.Commands.UpdateDispensers;

    public record UpdateDispensersCommand (
    int Id,
    string Code,
    int Number,
    int DispenserTypeId,
    int HoseNumber,
    int EdsId,
    int IdIsland) : IRequest<Result<VoidResult, Error>>;
