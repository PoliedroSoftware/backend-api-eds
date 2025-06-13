using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Compartiment.Commands.UpdateCompartiment;

    public record UpdateCompartimentCommand(
    int IdCompartment,
    int Number,
    double Nominal,
    double Operative,
    double Stock,
    double Height,
    int IdTank) : IRequest<Result<VoidResult, Error>>;
