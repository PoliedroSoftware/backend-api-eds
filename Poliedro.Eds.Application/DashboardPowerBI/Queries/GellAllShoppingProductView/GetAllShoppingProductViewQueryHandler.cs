using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllShoppingProductView;
using Poliedro.Eds.Domain.ShoppingProductView.DomainShoppingProductView;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllShoppingProduct;
public class GetAllShoppingProductViewQueryHandler
(
    IShoppingProductGetAllShoppingProductView shoppingProductDomainService,
    IMapper mapper)
    : IRequestHandler<GellAllShoppingProductViewQuery, IEnumerable<ShoppingProductViewDto>>
{
    public async Task<IEnumerable<ShoppingProductViewDto>> Handle(GellAllShoppingProductViewQuery request, CancellationToken cancellationToken)
    {
        var result = await shoppingProductDomainService.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<ShoppingProductViewDto>>(result);
    }
}


