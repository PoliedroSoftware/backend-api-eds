using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.Dispensers.Entities;

namespace Poliedro.Eds.Domain.Dispensers.DomainDispensers
{
    public interface IDispensersGetAllDispensers
    {
        Task<IEnumerable<DispensersEntity>> GetAllAsync(PaginationParams paginationParams);
    }
}
