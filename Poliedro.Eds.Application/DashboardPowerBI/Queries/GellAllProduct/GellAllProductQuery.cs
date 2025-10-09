using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllProduct;

public record GellAllProductQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<ProductDto>>;
