using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Capacity.Entities;

namespace Poliedro.Eds.Domain.Capacity.DomainCapacity;

public interface ICapacityGetAllService
{
    Task<IEnumerable<CapacityEntity>> GetAllAsync(PaginationParams paginationParams);
}