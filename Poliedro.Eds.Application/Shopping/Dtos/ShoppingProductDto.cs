namespace Poliedro.Eds.Application.Shopping.Dtos;

public record ShoppingProductDto(
    int IdShoppingProduct,
    int IdShopping,
    int IdProduct,
    decimal Quantity,
    decimal PurchasePrice,
    decimal SellPrice,
    int IdCompartment
);
