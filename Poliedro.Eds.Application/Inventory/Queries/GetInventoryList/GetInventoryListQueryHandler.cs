using MediatR;
using Poliedro.Eds.Domain.Inventory.DomainService;
using Poliedro.Eds.Domain.Inventory.Dto.View;

namespace Poliedro.Eds.Application.Inventory.Queries.GetInventoryList;

public class GetInventoryListQueryHandler(
    IInventoryListDomainService inventoryListDomainService
) : IRequestHandler<GetInventoryListQuery, IEnumerable<InventoryListResponseDto>>
{
    public async Task<IEnumerable<InventoryListResponseDto>> Handle(GetInventoryListQuery request, CancellationToken cancellationToken)
        => await inventoryListDomainService.GetAllAsync(request.PaginationParams);
}
