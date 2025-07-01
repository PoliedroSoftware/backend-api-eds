namespace Poliedro.Eds.Application.Shopping.Commands.CreateShopping;

public record ShoppingProductRequestDto(
    int IdProduct,
    double Quantity,
    double Price,
    int IdCompartment
);
