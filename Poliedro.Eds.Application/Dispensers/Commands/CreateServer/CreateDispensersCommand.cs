using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Dispensers.Commands.CreateDispensers;

public record CreateDispensersCommand(CreateDispensersRequestDto Request) : IRequest<Result<VoidResult, Error>>;