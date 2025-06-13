using MediatR;
using Poliedro.Eds.Application.Shopping.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.Shopping.Queries.GellAllShopping;
public record GellAllShoppingQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<ShoppingDto>>;
