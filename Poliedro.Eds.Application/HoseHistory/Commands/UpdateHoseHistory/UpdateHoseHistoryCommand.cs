using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;


namespace Poliedro.Eds.Application.HoseHistory.Commands.UpdateHoseHistory;

public record UpdateHoseHistoryCommand(
    int IdHoseHistory,
    int IdHose,
    int IdDispensers,
    double AccumulatedGallons,
    double AccumulatedAmount,
    DateTime Date) : IRequest<Result<VoidResult, Error>>;