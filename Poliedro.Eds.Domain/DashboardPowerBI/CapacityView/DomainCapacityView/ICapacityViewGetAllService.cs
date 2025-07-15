using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityView.Entities;

namespace Poliedro.Eds.Domain.DashboardPowerBI.CapacityView.DomainCapacityView;

public interface ICapacityViewGetAllService
{
    Task<IEnumerable<CapacityViewEntity>> GetAllAsync(PaginationParams paginationParams);
}