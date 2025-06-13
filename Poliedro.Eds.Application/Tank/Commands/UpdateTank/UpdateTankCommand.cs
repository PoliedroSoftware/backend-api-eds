using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;


namespace Poliedro.Eds.Application.Tank.Commands.UpdateTank;

public record UpdateTankCommand(
   int IdTank,
   string Number,
   int Compartment,
   double Ability,
   double Stock) : IRequest<Result<VoidResult, Error>>;