namespace Poliedro.Eds.Application.Inventory.Dtos;

public class InventoryDto
{
    public int IdInventory { get; set; }
    public DateOnly Date { get; set; }
    public string ReferenceType { get; set; }
    public int ReferenceId { get; set; }
}