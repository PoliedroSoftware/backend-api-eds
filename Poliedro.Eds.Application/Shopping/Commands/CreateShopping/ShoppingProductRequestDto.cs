namespace Poliedro.Eds.Application.Shopping.Commands.CreateShopping;

public record ShoppingProductRequestDto(
    int IdProduct,
    decimal Quantity,
    decimal PurchasePrice,
    decimal SellPrice,
    int IdCompartment
);
