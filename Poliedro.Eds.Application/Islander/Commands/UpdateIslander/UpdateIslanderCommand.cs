using MediatR;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Common.Results;


namespace Poliedro.Eds.Application.Islander.Commands.UpdateIslander;
public record UpdateIslanderCommand(
    int IdIslander,
    int IdEds,
    string Name) : IRequest<Result<VoidResult, Error>>;
