using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.Entities;

namespace Poliedro.Eds.Domain.Court.DomainService;

public interface ICourtUpdateInventoryService
{
    Task<Result<VoidResult, Error>> CourtUpdateInventoryAsync(IEnumerable<CourtDispenserSaleEntity> courtDispensers);
}
