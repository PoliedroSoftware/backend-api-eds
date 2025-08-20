using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;


namespace Poliedro.Eds.Application.Island.Commands.UpdateIsland;

public record UpdateIslandCommand(
    int IdIsland,
    string Description) : IRequest<Result<VoidResult, Error>>;
