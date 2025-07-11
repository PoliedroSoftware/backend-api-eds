using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Islander.Commands.CreateIslander;

public record CreateIslanderCommand(CreateIslanderRequestDto Request) : IRequest<Result<VoidResult, Error>>;


