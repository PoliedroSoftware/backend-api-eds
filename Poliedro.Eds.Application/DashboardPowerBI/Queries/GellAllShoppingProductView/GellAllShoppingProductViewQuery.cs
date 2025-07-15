using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllShoppingProductView;
public record GellAllShoppingProductViewQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<ShoppingProductViewDto>>;
