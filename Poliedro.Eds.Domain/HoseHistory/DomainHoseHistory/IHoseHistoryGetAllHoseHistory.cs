using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.HoseHistory.Entities;

namespace Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory
{
    public interface IHoseHistoryGetAllHoseHistory
    {
        Task<IEnumerable<HoseHistoryEntity>> GetAllAsync(PaginationParams paginationParams);
    }
}
