using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.ProductHistory.Entities;

public class ProductHistoryEntity : AuditableEntity
{
    [Key]
    public int IdProductHistory { get; set; }
    public int IdProduct { get; set; }
    public string Name { get; set; } = default!;
    public int IdProductType { get; set; }
    public double? PurchasePrice { get; set; }
    public double? SellPrice { get; set; }
    public double? Stock { get; set; }
    public DateTime Date { get; set; }
}
