using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.Entities.Master;

namespace Poliedro.Eds.Domain.DashboardPowerBI.DomainDashboardPowerBI;

public interface IMasterGetAllService
{
    Task<MasterEntity> GetAllAsync(
        PaginationParams paginationParams,
        CancellationToken cancellationToken);
}
