using MediatR;
using Poliedro.Eds.Application.ProductType.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.ProductType.Queries.GellAllProductType;

public record GellAllProductTypeQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<ProductTypeDto>>;
