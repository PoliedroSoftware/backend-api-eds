using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;
<<<<<<< HEAD
using Poliedro.Eds.Domain.ProductType.Entities;
=======
>>>>>>> New-service-StrongBox

namespace Poliedro.Eds.Domain.Product.Entities;

public class ProductEntity : AuditableEntity
{
    [Key]
    public int IdProduct { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int IdProductType { get; set; }
<<<<<<< HEAD
    public double PurchasePrice { get; set; }
    public double SellPrice { get; set; }
    public double Stock { get; set; }
    public DateTime Date { get; set; }

    // Navigation property
    public virtual ProductTypeEntity? ProductType { get; set; }
=======
    public double Price { get; set; }
>>>>>>> New-service-StrongBox
}
