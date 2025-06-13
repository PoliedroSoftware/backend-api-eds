using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Tank.Entities;

namespace Poliedro.Eds.Domain.Tank.DomainTank;
public interface ITankGetAllTank
{  
    Task<IEnumerable<TankEntity>> GetAllAsync(PaginationParams paginationParams);
}