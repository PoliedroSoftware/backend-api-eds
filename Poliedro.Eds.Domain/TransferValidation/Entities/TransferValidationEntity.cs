using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.TransferValidation.Entities;

public class TransferValidationEntity : AuditableEntity
{
    [Key]
    public int IdTransferValidation { get; set; }
    
    public string UniqueId { get; set; } = null!;
    
    public string CustomerName { get; set; } = null!;
    
    public double TransactionAmount { get; set; }
    
    public DateOnly TransactionDate { get; set; }
    
    public TimeOnly TransactionTime { get; set; }
    
    public string Status { get; set; } = null!;
    
    public string? ConfirmedBy { get; set; }

    private TransferValidationEntity() { }

    public TransferValidationEntity(
        string uniqueId,
        string customerName,
        double transactionAmount,
        DateOnly transactionDate,
        TimeOnly transactionTime,
        string status,
        string? confirmedBy = null)
    {
        if (string.IsNullOrWhiteSpace(uniqueId))
            throw new ArgumentException("El ID único es requerido", nameof(uniqueId));

        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("El nombre del cliente es requerido", nameof(customerName));

        if (transactionAmount <= 0)
            throw new ArgumentException("El monto de la transacción debe ser mayor a cero", nameof(transactionAmount));

        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("El estado es requerido", nameof(status));

        UniqueId = uniqueId.Trim();
        CustomerName = customerName.Trim();
        TransactionAmount = double.Round(transactionAmount, 2);
        TransactionDate = transactionDate;
        TransactionTime = transactionTime;
        Status = status.Trim();
        ConfirmedBy = confirmedBy?.Trim();
    }

    public void UpdateConfirmation(string confirmedBy)
    {
        if (string.IsNullOrWhiteSpace(confirmedBy))
            throw new ArgumentException("El usuario que confirma es requerido", nameof(confirmedBy));

        ConfirmedBy = confirmedBy.Trim();
        UpdatedAt = DateTime.Now;
    }


    public void UpdateStatus(string newStatus)
    {
        if (string.IsNullOrWhiteSpace(newStatus))
            throw new ArgumentException("El nuevo estado es requerido", nameof(newStatus));

        Status = newStatus.Trim();
        UpdatedAt = DateTime.Now;
    }
}
