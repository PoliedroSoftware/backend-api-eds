using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Hose.Commands.CreateHose;

public record CreateHoseCommand(CreateHoseRequestDto Request) : IRequest<Result<VoidResult, Error>>;
