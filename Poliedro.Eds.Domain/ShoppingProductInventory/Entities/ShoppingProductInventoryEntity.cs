using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.ShoppingProductInventory.Entities
{
    public class ShoppingProductInventoryEntity : AuditableEntity
    {
        public int IdShoppingProductInventory { get; set; }
        public int IdShoppingProduct { get; set; }
        public int IdInventory { get; set; }
    }
}
