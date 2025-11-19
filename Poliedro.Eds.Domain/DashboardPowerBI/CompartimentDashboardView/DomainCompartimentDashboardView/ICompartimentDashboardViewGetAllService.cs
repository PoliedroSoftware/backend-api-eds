using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.CompartimentDashboardView.Entities;

namespace Poliedro.Eds.Domain.DashboardPowerBI.CompartimentDashboardView.DomainCompartimentDashboardView;

public interface ICompartimentDashboardViewGetAllService
{
    Task<IEnumerable<CompartimentDashboardViewEntity>> GetAllAsync(PaginationParams paginationParams);
}
