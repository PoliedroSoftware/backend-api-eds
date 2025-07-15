using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.CompartimentView.Entities;
namespace Poliedro.Eds.Domain.Compartiment.DomainCompartimentView
{
    public interface ICompartimentGetAllService
    {
        Task<IEnumerable<CompartimentViewEntity>> GetAllAsync(PaginationParams paginationParams);
    }
}