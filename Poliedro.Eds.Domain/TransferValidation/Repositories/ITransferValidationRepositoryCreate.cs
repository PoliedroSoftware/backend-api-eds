using Poliedro.Eds.Domain.TransferValidation.Entities;

namespace Poliedro.Eds.Domain.TransferValidation.Repositories;

public interface ITransferValidationRepositoryCreate
{

    Task<TransferValidationEntity> CreateAsync(
        TransferValidationEntity entity, 
        CancellationToken cancellationToken = default);
}
