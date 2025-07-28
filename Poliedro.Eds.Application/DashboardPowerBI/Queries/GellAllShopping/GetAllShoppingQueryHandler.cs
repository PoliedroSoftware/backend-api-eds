using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.Shopping.DomainShopping;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllShopping;

public class GetAllShoppingQueryHandler
(
    IShoppingGetAllShopping shoppingDomainService,
    IMapper mapper)
    : IRequestHandler<GellAllShoppingQuery, IEnumerable<ShoppingDto>>
{
    public async Task<IEnumerable<ShoppingDto>> Handle(GellAllShoppingQuery request, CancellationToken cancellationToken)
    {
        var result = await shoppingDomainService.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<ShoppingDto>>(result);
    }
}


