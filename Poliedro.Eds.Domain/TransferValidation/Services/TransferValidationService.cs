using Poliedro.Eds.Domain.TransferValidation.Entities;
using Poliedro.Eds.Domain.TransferValidation.Exceptions;
using Poliedro.Eds.Domain.TransferValidation.Repositories;
using Poliedro.Eds.Domain.TransferValidation.ValueObjects;

namespace Poliedro.Eds.Domain.TransferValidation.Services;

public class TransferValidationService(
    ITransferValidationRepositoryCreate repositoryCreate,
    ITransferValidationRepositoryGetByUniqueId repositoryGetByUniqueId,
    ITransferValidationRepositoryGetById repositoryGetById,
    ITransferValidationRepositoryUpdate repositoryUpdate) 
    : ITransferValidationService
{
    public async Task<TransferValidationEntity> CreateAsync(
        string uniqueId,
        string customerName,
        double transactionAmount,
        DateOnly transactionDate,
        TimeOnly transactionTime,
        string status,
        string? confirmedBy,
        CancellationToken cancellationToken = default)
    {
        // Validaciones de negocio
        if (string.IsNullOrWhiteSpace(uniqueId))
            throw new TransferValidationDomainException("El ID único es requerido.");

        if (string.IsNullOrWhiteSpace(customerName))
            throw new TransferValidationDomainException("El nombre del cliente es requerido.");

        if (transactionAmount <= 0)
            throw new TransferValidationDomainException("El monto de la transacción debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(status))
            throw new TransferValidationDomainException("El estado es requerido.");

        var normalizedStatus = TransferValidationStatus.Normalize(status);

        // Verificar que el uniqueId no exista
        var existingTransfer = await repositoryGetByUniqueId.GetByUniqueIdAsync(
            uniqueId.Trim(), 
            cancellationToken);

        if (existingTransfer != null)
        {
            throw new TransferValidationDomainException(
                $"Ya existe una validación de transferencia con el ID único: {uniqueId}");
        }

        // Validar que si el estado es CONFIRMADA, debe haber un confirmedBy
        if (normalizedStatus == TransferValidationStatus.CONFIRMADA && 
            string.IsNullOrWhiteSpace(confirmedBy))
        {
            throw new TransferValidationDomainException(
                "Si el estado es CONFIRMADA, debe especificar quién confirmó la transacción.");
        }

        // Crear la entidad
        var entity = new TransferValidationEntity(
            uniqueId: uniqueId.Trim(),
            customerName: customerName.Trim(),
            transactionAmount: transactionAmount,
            transactionDate: transactionDate,
            transactionTime: transactionTime,
            status: normalizedStatus,
            confirmedBy: confirmedBy?.Trim()
        );

        // Persistir en la base de datos
        return await repositoryCreate.CreateAsync(entity, cancellationToken);
    }

    public async Task<TransferValidationEntity> UpdateAsync(
        int idTransferValidation,
        string customerName,
        double transactionAmount,
        DateOnly transactionDate,
        TimeOnly transactionTime,
        string status,
        string? confirmedBy,
        CancellationToken cancellationToken = default)
    {
        // Validaciones de negocio
        if (idTransferValidation <= 0)
            throw new TransferValidationDomainException("El ID de la validación de transferencia debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(customerName))
            throw new TransferValidationDomainException("El nombre del cliente es requerido.");

        if (transactionAmount <= 0)
            throw new TransferValidationDomainException("El monto de la transacción debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(status))
            throw new TransferValidationDomainException("El estado es requerido.");

        var normalizedStatus = TransferValidationStatus.Normalize(status);

        // Verificar que la entidad exista
        var existingEntity = await repositoryGetById.GetByIdAsync(idTransferValidation, cancellationToken);
        if (existingEntity == null)
        {
            throw new TransferValidationDomainException(
                $"No se encontró la validación de transferencia con ID: {idTransferValidation}");
        }

        // Validar que si el estado es CONFIRMADA, debe haber un confirmedBy
        if (normalizedStatus == TransferValidationStatus.CONFIRMADA && 
            string.IsNullOrWhiteSpace(confirmedBy))
        {
            throw new TransferValidationDomainException(
                "Si el estado es CONFIRMADA, debe especificar quién confirmó la transacción.");
        }

        // Actualizar la entidad existente
        existingEntity.CustomerName = customerName.Trim();
        existingEntity.TransactionAmount = transactionAmount;
        existingEntity.TransactionDate = transactionDate;
        existingEntity.TransactionTime = transactionTime;
        existingEntity.Status = normalizedStatus;
        existingEntity.ConfirmedBy = confirmedBy?.Trim();
        existingEntity.UpdatedAt = DateTime.Now;

        // Persistir los cambios
        return await repositoryUpdate.UpdateAsync(existingEntity, cancellationToken);
    }
}
