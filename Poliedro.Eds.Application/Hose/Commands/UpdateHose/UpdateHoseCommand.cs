using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;


namespace Poliedro.Eds.Application.Hose.Commands.UpdateHose;

public record UpdateHoseCommand(
    int IdHose,
<<<<<<< HEAD
    int IdDispensers,
    int Number,    
    double AccumulatedAmount,
    double AccumulatedGallons,
    int IdProductType,
    int IdCompartiment) : IRequest<Result<VoidResult, Error>>;
=======
    int Number,
    int IdDispensers,
    double AccumulatedGallons,
    double AccumulatedAmount,
    int IdProductType) : IRequest<Result<VoidResult, Error>>;
>>>>>>> New-service-StrongBox
