using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.DashboardPowerBI.EdsView.Entities;

namespace Poliedro.Eds.Domain.DashboardPowerBI.EdsView.DomainEds;

public interface IEdsViewGetAllService
{
    Task<IEnumerable<EdsViewEntity>> GetAllAsync(PaginationParams paginationParams);
}
