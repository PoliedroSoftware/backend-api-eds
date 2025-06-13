
namespace Poliedro.Eds.Application.ShoppingProduct.Dtos;

public record ShoppingProductDto(
    int IdShoppingProduct,
    int IdShopping,
    int IdProduct,
    double Quantity,
    double Price ,
    int IdCompartment
    );
