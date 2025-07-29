using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.ShoppingProduct.Entities;

public class ShoppingProductEntity : AuditableEntity
{
    [Key]
    public int IdShoppingProduct { get; set; }
    public int IdShopping { get; set; }
    public int IdProduct { get; set; }
    public double Quantity { get; set; }
    public double Price { get; set; }
    public int IdCompartment { get; set; }
}


