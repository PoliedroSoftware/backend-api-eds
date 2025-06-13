using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.ProductType.Entities;

public class ProductTypeEntity
{
    [Key]
    public int IdProductType { get; set; } = default!;
    public string Description { get; set; } = default!;
}
