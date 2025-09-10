
namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos;

public record ShoppingProductViewDto(
    string IdShoppingProduct,
    string IdShopping,
    string IdProduct,
    DateOnly Date,
    string ProductName,
    double Quantity,
    double SellPrice,
    double PurchasePrice,
    double Stock,
    string IdCompartment,
    double TotalSellPrice,
    double TotalPurchasePrice,
    string IdBusiness
    );
