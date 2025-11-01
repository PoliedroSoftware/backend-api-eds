using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Compartiment.Commands.UpdateCompartiment;

public record UpdateCompartimentCommand(
int IdCompartiment,
int Number,
double Nominal,
double Operative,
int IdProduct,
double Height,
int IdTank) : IRequest<Result<VoidResult, Error>>;
