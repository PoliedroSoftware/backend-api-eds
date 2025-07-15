using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.ProviderView.Entities;

namespace Poliedro.Eds.Domain.DashboardPowerBI.ProviderView.DomainProviderView;

public interface IProviderViewGetAllService
{
    Task<IEnumerable<ProviderViewEntity>> GetAllAsync(PaginationParams paginationParams);
}