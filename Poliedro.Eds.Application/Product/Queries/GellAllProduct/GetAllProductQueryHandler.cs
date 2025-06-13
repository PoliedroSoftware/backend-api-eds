using System.Diagnostics.Metrics;
using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Domain.Product.DomainProduct;

namespace Poliedro.Eds.Application.Product.Queries.GellAllProduct;
public class GetAllProductQueryHandler
(
    IProductGetAllProduct ProductDomainProduct,
    IMapper mapper)
    : IRequestHandler<GellAllProductQuery, IEnumerable<ProductDto>>
{
    public async Task<IEnumerable<ProductDto>> Handle(GellAllProductQuery request, CancellationToken cancellationToken)
    {
        var result = await ProductDomainProduct.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<ProductDto>>(result);
    }
}


