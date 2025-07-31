using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.Inventory.Entities;

public class InventoryEntity : AuditableEntity
{
    [Key]
    public int IdInventory { get; set; }
    public DateOnly Date { get; set; }
    public string ReferenceType { get; set; }
    public int ReferenceId { get; set; }
}
