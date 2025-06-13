using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Domain.Eds.DomainEds;

public interface IEdsGetAllService
{
    Task<IEnumerable<EdsEntity>> GetAllAsync(PaginationParams paginationParams);
}