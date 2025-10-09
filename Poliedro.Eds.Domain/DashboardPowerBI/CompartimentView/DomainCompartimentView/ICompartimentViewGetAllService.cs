using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.DashboardPowerBI.CompartimentView.Entities;

namespace Poliedro.Eds.Domain.DashboardPowerBI.CompartimentView.DomainCompartimentView
{
    public interface ICompartimenViewGetAllService
    {
        Task<IEnumerable<CompartimentViewEntity>> GetAllAsync(PaginationParams paginationParams);
    }
}
