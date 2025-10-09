using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;

namespace Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory
{
    public interface ICourtDispensersInventoryGetAllCourtDispensersInventory
    {
        Task<IEnumerable<CourtDispensersInventoryEntity>> GetAllAsync(PaginationParams paginationParams);
    }
}
