using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.Entities;

namespace Poliedro.Eds.Domain.Court.DomainService
{
    public interface ICourtTransactionalService
    {
        Task<Result<CourtEntity, Error>> ExecuteCourtTransactionAsync(
            CourtEntity courtEntity,
            IEnumerable<CourtDispenserSaleEntity> courtDispenserSaleEntities,
            CancellationToken cancellationToken = default);

        Task<Result<CourtEntity, Error>> ExecuteCourtTransactionWithPriceValidationAsync(
            CourtEntity courtEntity,
            IEnumerable<CourtDispenserTransactionData> courtDispensers,
            IEnumerable<CourtDispenserSaleEntity> courtDispenserSaleEntities,
            CancellationToken cancellationToken = default);
    }
}
