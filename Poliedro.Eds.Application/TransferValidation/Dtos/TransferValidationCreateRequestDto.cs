namespace Poliedro.Eds.Application.TransferValidation.Dtos;

public class TransferValidationCreateRequestDto
{
    public string UniqueId { get; set; } = null!;
    
    public string CustomerName { get; set; } = null!;
    
    public double TransactionAmount { get; set; }
    
    public DateOnly TransactionDate { get; set; }
    
    public TimeOnly TransactionTime { get; set; }
    
    public string Status { get; set; } = null!;
    
    public string? ConfirmedBy { get; set; }
}
