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
    public decimal Amount { get; set; }

    // Nueva columna para multi-tenant por EDS
    public int? IdEds { get; set; }

    public IEnumerable<ShoppingProductEntity> ShoppingProducts { get; set; }
    public InventoryEntity ShoppingInventory { get; set; }
}



