using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.HoseHistory.Commands.CreateHoseHistory;

public record CreateHoseHistoryCommand(CreateHoseHistoryRequestDto Request) : IRequest<Result<VoidResult, Error>>;
