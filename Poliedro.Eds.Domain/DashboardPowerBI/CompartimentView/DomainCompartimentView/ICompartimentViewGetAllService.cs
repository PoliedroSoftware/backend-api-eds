using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.CompartimentView.Entities;

namespace Poliedro.Eds.Domain.DashboardPowerBI.CompartimentView.DomainCompartimentView
{
    public interface ICompartimenViewGetAllService
    {
        Task<IEnumerable<CompartimentViewEntity>> GetAllAsync(PaginationParams paginationParams);
    }
}
