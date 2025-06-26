namespace Poliedro.Eds.Application.Shopping.Dtos;

public record ShoppingProductDto(
    int IdShoppingProduct,
    int IdShopping,
    int IdProduct,
    double Quantity,
    double Price,
    int IdCompartment
);