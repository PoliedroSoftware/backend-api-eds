using MediatR;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Application.ShoppingProduct.Queries.GellAllShoppingProduct;

public record GellAllShoppingProductQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<ShoppingProductDto>>;
