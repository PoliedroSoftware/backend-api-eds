using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.Inventory.Entities;

public class InventoryEntity
{
    [Key]
    public int IdInventory { get; set; }
    public DateOnly Date { get; set; }
    public string ReferenceType { get; set; }
    public int ReferenceId { get; set; }
}