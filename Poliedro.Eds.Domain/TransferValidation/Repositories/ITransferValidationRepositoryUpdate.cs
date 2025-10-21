using Poliedro.Eds.Domain.TransferValidation.Entities;

namespace Poliedro.Eds.Domain.TransferValidation.Repositories;

public interface ITransferValidationRepositoryUpdate
{
    Task<TransferValidationEntity> UpdateAsync(
        TransferValidationEntity entity, 
        CancellationToken cancellationToken = default);
}
