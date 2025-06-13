using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;

namespace Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory
{
    public interface ICourtDispensersInventoryGetByIdCourtDispensersInventory
    {
        Task<Result<CourtDispensersInventoryEntity, Error>> GetByIdAsync(int id);   
    }
}

