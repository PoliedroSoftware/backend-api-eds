using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;
<<<<<<< HEAD
using Poliedro.Eds.Domain.Product.Entities;
=======
>>>>>>> New-service-StrongBox

namespace Poliedro.Eds.Domain.ProductType.Entities;

public class ProductTypeEntity : AuditableEntity
{
    [Key]
    public int IdProductType { get; set; } = default!;
    public string Description { get; set; } = default!;
<<<<<<< HEAD

    // Navigation property
    public virtual ICollection<ProductEntity>? Products { get; set; }
=======
>>>>>>> New-service-StrongBox
}
