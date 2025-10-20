using Poliedro.Eds.Domain.TransferValidation.Entities;

namespace Poliedro.Eds.Domain.TransferValidation.Services;

public interface ITransferValidationService
{
    Task<TransferValidationEntity> CreateAsync(
        string uniqueId,
        string customerName,
        double transactionAmount,
        DateOnly transactionDate,
        TimeOnly transactionTime,
        string status,
        string? confirmedBy,
        CancellationToken cancellationToken = default);

    Task<TransferValidationEntity> UpdateAsync(
        int idTransferValidation,
        string customerName,
        double transactionAmount,
        DateOnly transactionDate,
        TimeOnly transactionTime,
        string status,
        string? confirmedBy,
        CancellationToken cancellationToken = default);
}
