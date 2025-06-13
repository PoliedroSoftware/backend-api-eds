namespace Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;

public record CreateShoppingProductRequestDto(
    int IdShopping,
    int IdProduct,
    double Quantity,
    double Price,
    int IdCompartment);


