using Poliedro.Eds.Application.Inventory.Commands;

namespace Poliedro.Eds.Application.Shopping.Commands.CreateShopping;

public record CreateShoppingRequestDto(
    string Invoice,
    DateTime Date,
    int IdProvider,
    int IdCategory,
    double Amount,
    IEnumerable<ShoppingProductRequestDto> ShoppingProducts,
    InventoryCommand? ShoppingInventory,
    int? IdEds
    );
