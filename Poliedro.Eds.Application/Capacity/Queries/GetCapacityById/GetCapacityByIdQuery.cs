using MediatR;
using Poliedro.Eds.Application.Capacity.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Capacity.Queries.GetCapacityById;
public record GetCapacityByIdQuery (int Id) : IRequest<Result<CapacityDto, Error>>;