using MediatR;
using Poliedro.Eds.Application.Capacity.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.Capacity.Queries.GellAllCapacity;

public record GellAllCapacityQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<CapacityDto>>;