using MediatR;
using Poliedro.Eds.Domain.Inventory.DomainService;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using AutoMapper;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GetInventoryList;

public class GetInventoryListQueryHandler(
    IInventoryListDomainService inventoryListDomainService,
     IMapper mapper
) : IRequestHandler<GetInventoryListQuery, IEnumerable<InventoryDto>>
{
    public async Task<IEnumerable<InventoryDto>> Handle(GetInventoryListQuery request, CancellationToken cancellationToken)
    {
        var result = await inventoryListDomainService.GetAllAsync(request.PaginationParams);
        return mapper.Map<IEnumerable<InventoryDto>>(result);
    }

}
