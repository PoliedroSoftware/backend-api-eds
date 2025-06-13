using MediatR;
using Poliedro.Eds.Application.ProductCompartiment.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.ProductCompartiment.Queries.GellAllProductCompartiment;

public record GellAllProductCompartimentQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<ProductCompartimentDto>>;
