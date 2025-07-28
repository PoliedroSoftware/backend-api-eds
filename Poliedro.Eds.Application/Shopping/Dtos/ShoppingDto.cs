
using Poliedro.Eds.Application.Inventory.Dtos;

namespace Poliedro.Eds.Application.Shopping.Dtos;

public class ShoppingDto
{
    public int IdShopping { get; set; }
    public string Invoice { get; set; }
    public DateTime Date { get; set; }
    public int IdProvider { get; set; }
    public int IdCategory { get; set; }
    public double Amount { get; set; }
    public IEnumerable<ShoppingProductDto> ShoppingProducts { get; set; }
    public InventoryDto? ShoppingInventory { get; set; }
}
