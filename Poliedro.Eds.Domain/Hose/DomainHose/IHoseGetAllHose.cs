using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Hose.Dtos;

namespace Poliedro.Eds.Domain.Hose.DomainHose;

public interface IHoseGetAllHose
{  
    Task<IEnumerable<HoseDto>> GetAllAsync(PaginationParams paginationParams);
}