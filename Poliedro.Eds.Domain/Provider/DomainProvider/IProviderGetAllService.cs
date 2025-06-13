using Poliedro.Eds.Domain.Provider.Entities;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Domain.Provider.DomainProvider;

public interface IProviderGetAllService
{
    Task<IEnumerable<ProviderEntity>> GetAllAsync(PaginationParams paginationParams);
}