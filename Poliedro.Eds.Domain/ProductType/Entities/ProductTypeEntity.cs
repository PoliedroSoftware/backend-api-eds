using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.ProductType.Entities;

public class ProductTypeEntity : AuditableEntity
{
    [Key]
    public int IdProductType { get; set; } = default!;
    public string Description { get; set; } = default!;
}
