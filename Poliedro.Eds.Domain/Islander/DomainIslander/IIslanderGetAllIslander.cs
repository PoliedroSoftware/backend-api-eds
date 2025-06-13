using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Islander.Entities;

namespace Poliedro.Eds.Domain.Islander.DomainIslander
{
    public interface IIslanderGetAllIslander
    {  
        Task<IEnumerable<IslanderEntity>> GetAllAsync(PaginationParams paginationParams);
    }
}