using System.Diagnostics.Metrics;
using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.ProductCompartiment.Dtos;
using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;

namespace Poliedro.Eds.Application.ProductCompartiment.Queries.GellAllProductCompartiment;
public class GetAllProductCompartimentQueryHandler
(
    IProductCompartimentGetAllProductCompartiment ProductCompartimentDomainProductCompartiment,
    IMapper mapper)
    : IRequestHandler<GellAllProductCompartimentQuery, IEnumerable<ProductCompartimentDto>>
{
    public async Task<IEnumerable<ProductCompartimentDto>> Handle(GellAllProductCompartimentQuery request, CancellationToken cancellationToken)
    {
        var result = await ProductCompartimentDomainProductCompartiment.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<ProductCompartimentDto>>(result);
    }
}


