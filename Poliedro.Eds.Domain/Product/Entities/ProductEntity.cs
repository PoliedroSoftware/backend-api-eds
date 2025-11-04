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
    public decimal? PurchasePrice { get; set; }
    public decimal? SellPrice { get; set; }
    public decimal? Stock { get; set; }
    public DateTime Date { get; set; }
    public int? IdEds { get; set; }


    public virtual ProductTypeEntity? ProductType { get; set; }
}
