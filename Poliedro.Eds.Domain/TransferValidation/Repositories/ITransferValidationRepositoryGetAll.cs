using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.TransferValidation.Entities;

namespace Poliedro.Eds.Domain.TransferValidation.Repositories;

public interface ITransferValidationRepositoryGetAll
{
    Task<IEnumerable<TransferValidationEntity>> GetAllAsync(
    PaginationParams paginationParams,
    CancellationToken cancellationToken = default);
}
