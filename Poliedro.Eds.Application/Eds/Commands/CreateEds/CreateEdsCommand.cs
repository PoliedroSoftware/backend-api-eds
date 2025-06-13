using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Eds.Commands.CreateEds;

public record CreateEdsCommand(CreateEdsRequestDto Request) : IRequest<Result<VoidResult, Error>>;