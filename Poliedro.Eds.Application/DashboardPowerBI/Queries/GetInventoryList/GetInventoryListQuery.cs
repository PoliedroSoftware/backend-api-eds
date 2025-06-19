using MediatR;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GetInventoryList;

public record GetInventoryListQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<InventoryDto>>;
