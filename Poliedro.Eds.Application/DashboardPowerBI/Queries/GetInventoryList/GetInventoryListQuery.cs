using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GetInventoryList;

public record GetInventoryListQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<InventoryDto>>;
