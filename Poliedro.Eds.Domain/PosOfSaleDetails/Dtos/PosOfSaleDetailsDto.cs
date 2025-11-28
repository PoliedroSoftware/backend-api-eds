using Poliedro.Eds.Domain.PosOfSale.Dtos;

namespace Poliedro.Eds.Domain.PosOfSaleDetails.Dtos;

public record PosOfSaleDetailsDto
{
    public int IdDetail { get; set; }

    public int PosId { get; set; }

    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public string UnitMeasure { get; set; } = "UND";
    public decimal Quantity { get; set; } = 0;
    public decimal UnitPrice { get; set; } = 0;

    public decimal DiscountAmount { get; set; } = 0;
    public decimal TaxAmount { get; set; } = 0;
    public decimal TotalAmount { get; set; } = 0;

    public string? TaxType { get; set; } = "IVA";
    public decimal TaxPercentage { get; set; } = 0;
    public string? RetentionType { get; set; }
    public decimal RetentionAmount { get; set; } = 0;

    public PosOfSaleDto? PosOfSale { get; set; }
}
