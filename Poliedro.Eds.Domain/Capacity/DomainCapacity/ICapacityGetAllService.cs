using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Domain.Capacity.DomainCapacity;

public interface ICapacityGetAllService
{
    Task<IEnumerable<CapacityEntity>> GetAllAsync(PaginationParams paginationParams);
}
