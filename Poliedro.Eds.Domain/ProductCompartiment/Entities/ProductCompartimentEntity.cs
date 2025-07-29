using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.ProductCompartiment.Entities;

public class ProductCompartimentEntity : AuditableEntity
{
    [Key]
    public int IdProductCompartiment { get; set; } = default!;
    public int IdProduct { get; set; } = default!;
    public int IdCompartiment { get; init; }
    public double Stock { get; init; }
}
