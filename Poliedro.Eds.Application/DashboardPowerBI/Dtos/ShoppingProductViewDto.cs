
namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos;

public record ShoppingProductViewDto(
    string IdShoppingProduct,
    string IdShopping,
    string IdProduct,
    DateOnly Date,
    string ProductName,
    decimal Quantity,
    decimal SellPrice,
    decimal PurchasePrice,
    decimal Stock,
    string IdCompartment,
    decimal TotalSellPrice,
    decimal TotalPurchasePrice,
    string IdBusiness
    );
