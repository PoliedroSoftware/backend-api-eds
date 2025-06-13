using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;


namespace Poliedro.Eds.Application.Hose.Commands.UpdateHose;

public record UpdateHoseCommand(
    int IdHose,
    int Number,
    int IdDispensers,
    double AccumulatedGallons,
    double AccumulatedAmount,
    int IdProductType) : IRequest<Result<VoidResult, Error>>;