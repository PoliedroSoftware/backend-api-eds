using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllShopping;

public record GellAllShoppingQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<ShoppingDto>>;
