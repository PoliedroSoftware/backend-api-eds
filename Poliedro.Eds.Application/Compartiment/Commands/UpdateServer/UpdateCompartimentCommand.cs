using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Compartiment.Commands.UpdateCompartiment;

public record UpdateCompartimentCommand(
int IdCompartment,
int Number,
decimal Nominal,
decimal Operative,
int IdProduct,
decimal Height,
int IdTank) : IRequest<Result<VoidResult, Error>>;
