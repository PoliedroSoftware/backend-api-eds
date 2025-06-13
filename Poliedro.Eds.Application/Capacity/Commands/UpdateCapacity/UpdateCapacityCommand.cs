using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Capacity.Commands.UpdateCapacity;

public record UpdateCapacityCommand(int IdCapacity, string Code, double Height, double Gallon, int Liters) : IRequest<Result<VoidResult, Error>>;