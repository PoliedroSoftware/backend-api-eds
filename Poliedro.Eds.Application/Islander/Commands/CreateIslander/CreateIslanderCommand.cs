using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Islander.Commands.CreateIslander;

public record CreateIslanderCommand(CreateIslanderRequestDto Request, string? NameClaimToken, int? IdEds = null)
    : IRequest<bool>;




