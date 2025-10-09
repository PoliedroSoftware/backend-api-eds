using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GetInventoryList;

public record GetInventoryListQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<InventoryDto>>;
