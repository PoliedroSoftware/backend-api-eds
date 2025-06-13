using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.CompartimentCapacity.Commands.CreateCompartimentCapacity;

public record CreateCompartimentCapacityCommand (CreateCompartimentCapacityRequestDto Request) : IRequest<Result<VoidResult, Error>>;
