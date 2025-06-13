using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Inventory.Dto.View;

namespace Poliedro.Eds.Domain.Inventory.DomainService;

public interface IInventoryListDomainService
{
    Task<IEnumerable<InventoryListResponseDto>> GetAllAsync(PaginationParams paginationParams);
}
