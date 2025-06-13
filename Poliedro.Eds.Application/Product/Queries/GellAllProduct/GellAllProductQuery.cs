using MediatR;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.Product.Queries.GellAllProduct;

public record GellAllProductQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<ProductDto>>;
