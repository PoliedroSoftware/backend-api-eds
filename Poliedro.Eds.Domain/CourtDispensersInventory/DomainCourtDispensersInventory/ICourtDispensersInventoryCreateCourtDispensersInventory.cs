using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;

namespace Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory
{
    public interface ICourtDispensersInventoryCreateCourtDispensersInventory
    {
        Task<Result<VoidResult, Error>> CreateAsync(CourtDispensersInventoryEntity CourtDispensersInventoryEntity);
       
    }
}

