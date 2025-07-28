using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.DashboardPowerBI.ProductView.DomainProductView;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllProduct;

public class GetAllProductQueryHandler
(
    IProductViewGetAllProduct ProductViewDomainProduct,
    IMapper mapper)
    : IRequestHandler<GellAllProductQuery, IEnumerable<ProductDto>>
{
    public async Task<IEnumerable<ProductDto>> Handle(GellAllProductQuery request, CancellationToken cancellationToken)
    {
        var result = await ProductViewDomainProduct.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<ProductDto>>(result);
    }
}


