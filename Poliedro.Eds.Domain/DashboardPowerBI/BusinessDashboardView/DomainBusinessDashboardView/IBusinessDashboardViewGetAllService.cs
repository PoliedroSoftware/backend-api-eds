using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.BusinessDashboardView.Entities;

namespace Poliedro.Eds.Domain.DashboardPowerBI.BusinessDashboardView.DomainBusinessDashboardView;

public interface IBusinessDashboardViewGetAllService
{
    Task<IEnumerable<BusinessDashboardViewEntity>> GetAllAsync(PaginationParams paginationParams);
}
