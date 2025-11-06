using MediatR;
using Poliedro.Eds.Application.Capacity.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Capacity.Commands.CreateCapacity;

public record CreateCapacityCommand(CreateCapacityRequestDto Request) : IRequest<Result<CapacityDto, Error>>;
