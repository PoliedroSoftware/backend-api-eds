using Poliedro.Eds.Domain.TransferValidation.Entities;

namespace Poliedro.Eds.Domain.TransferValidation.Repositories;

public interface ITransferValidationRepositoryGetByUniqueId
{

    Task<TransferValidationEntity?> GetByUniqueIdAsync(
        string uniqueId, 
        CancellationToken cancellationToken = default);
}
