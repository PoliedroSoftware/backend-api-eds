using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.DashboardPowerBI.BusinessView.Entities;

namespace Poliedro.Eds.Domain.DashboardPowerBI.BusinessView.DomainBusinessView;

public interface IBusinessViewGetAllService
{
    Task<IEnumerable<BusinessViewEntity>> GetAllAsync(PaginationParams paginationParams);
}
