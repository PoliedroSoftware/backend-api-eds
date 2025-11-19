using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityDashboardView.Entities;

namespace Poliedro.Eds.Domain.DashboardPowerBI.CapacityDashboardView.DomainCapacityDashboardView;

public interface ICapacityDashboardViewGetAllService
{
    Task<IEnumerable<CapacityDashboardViewEntity>> GetAllAsync(PaginationParams paginationParams);
}
