using Poliedro.Eds.Domain.Audit.Entities;
using Poliedro.Eds.Domain.Inventory.Entities;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;

namespace Poliedro.Eds.Domain.Shopping.Entities;

public class ShoppingEntity : AuditableEntity
{
    public int IdShopping { get; set; }
    public string? Invoice { get; set; }
    public DateTime Date { get; set; }
    public int IdProvider { get; set; }
    public int IdCategory { get; set; }
    public double Amount { get; set; }
    public int? IdEds { get; set; }
    public ICollection<ShoppingProductEntity> ShoppingProducts { get; set; } = new List<ShoppingProductEntity>();
    public InventoryEntity ShoppingInventory { get; set; }
}





