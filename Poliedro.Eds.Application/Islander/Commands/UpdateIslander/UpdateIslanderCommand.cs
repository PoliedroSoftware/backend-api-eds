using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;


namespace Poliedro.Eds.Application.Islander.Commands.UpdateIslander;

public record UpdateIslanderCommand(
    int IdIslander,
    int IdEds,
    string Name) : IRequest<Result<VoidResult, Error>>;
