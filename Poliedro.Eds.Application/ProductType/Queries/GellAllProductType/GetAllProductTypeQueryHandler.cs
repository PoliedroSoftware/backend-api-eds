using System.Diagnostics.Metrics;
using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.ProductType.Dtos;
using Poliedro.Eds.Domain.ProductType.DomainProductType;

namespace Poliedro.Eds.Application.ProductType.Queries.GellAllProductType;
public class GetAllProductTypeQueryHandler
(
    IProductTypeGetAllProductType ProductTypeDomainProductType,
    IMapper mapper)
    : IRequestHandler<GellAllProductTypeQuery, IEnumerable<ProductTypeDto>>
{
    public async Task<IEnumerable<ProductTypeDto>> Handle(GellAllProductTypeQuery request, CancellationToken cancellationToken)
    {
        var result = await ProductTypeDomainProductType.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<ProductTypeDto>>(result);
    }
}


