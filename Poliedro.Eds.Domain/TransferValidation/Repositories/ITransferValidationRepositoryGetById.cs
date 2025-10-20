using Poliedro.Eds.Domain.TransferValidation.Entities;

namespace Poliedro.Eds.Domain.TransferValidation.Repositories;

public interface ITransferValidationRepositoryGetById
{
    Task<TransferValidationEntity?> GetByIdAsync(
        int idTransferValidation, 
        CancellationToken cancellationToken = default);
}
