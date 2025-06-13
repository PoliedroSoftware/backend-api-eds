using Poliedro.Eds.Domain.Court.Entities;

namespace Poliedro.Eds.Domain.Court.DomainService;

public interface ICourtUpdateInventoryService
{
    Task CourtUpdateInventoryAsync(IEnumerable<ICourtDispenserSaleEntity> courtDispensers);
}
