using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.ProductHistory.Entities;

public class ProductHistoryEntity : AuditableEntity
{
    [Key]
    public int IdProductHistory { get; set; }
    public int IdProduct { get; set; }
    public double? OldSellPrice { get; set; }
    public double? NewSellPrice { get; set; }
    public double? OldStock { get; set; }
    public double? NewStock { get; set; }
    public double? OldPurchasePrice { get; set; }
    public double? NewPurchasePrice { get; set; }
    public DateTime Date { get; set; }
}
