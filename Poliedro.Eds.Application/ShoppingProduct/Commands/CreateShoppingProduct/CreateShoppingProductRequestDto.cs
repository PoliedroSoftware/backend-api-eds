using Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;

namespace Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;

public record CreateShoppingProductRequestDto(
    int IdShopping,
    int IdProduct,
    double Quantity,
    double PurchasePrice,
    double SellPrice,
    int IdCompartment);
