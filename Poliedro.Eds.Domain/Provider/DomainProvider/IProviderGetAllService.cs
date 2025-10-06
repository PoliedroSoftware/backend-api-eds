using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.Provider.Entities;

namespace Poliedro.Eds.Domain.Provider.DomainProvider;

public interface IProviderGetAllService
{
    Task<IEnumerable<ProviderEntity>> GetAllAsync(PaginationParams paginationParams);
}
