using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.ProductCompartiment.Entities;

public class ProductCompartimentEntity
{
    [Key]
    public int IdProductCompartiment { get; set; } = default!;
    public int IdProduct { get; set; } = default!;
    public int IdCompartiment { get; init; }
    public double Stock { get; init; }
}
