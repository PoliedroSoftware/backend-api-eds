using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllCapacity;

public record GellAllCapacityQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<CapacityDto>>;
