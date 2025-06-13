using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Island.Commands.CreateIsland;

public record CreateIslandCommand(CreateIslandRequestDto Request) : IRequest<Result<VoidResult, Error>>;


