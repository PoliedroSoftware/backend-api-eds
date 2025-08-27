using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;
using Poliedro.Eds.Domain.ProductType.Entities;

namespace Poliedro.Eds.Domain.Product.Entities;

public class ProductEntity : AuditableEntity
{
    [Key]
    public int IdProduct { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int IdProductType { get; set; }
    public double PurchasePrice { get; set; }
    public double SellPrice { get; set; }
    public double Stock { get; set; }
    public DateTime Date { get; set; }


    public virtual ProductTypeEntity? ProductType { get; set; }
}
