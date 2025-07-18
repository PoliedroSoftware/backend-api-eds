using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.Product.Entities;

public class ProductEntity
{
    [Key]
    public int IdProduct { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int IdProductType { get; set; }
    public double Price { get; set; }
    public DateTime Date { get; set; }
}
