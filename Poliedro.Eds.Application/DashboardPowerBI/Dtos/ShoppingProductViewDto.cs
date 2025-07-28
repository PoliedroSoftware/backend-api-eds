
namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos;

public record ShoppingProductViewDto(
    string IdShoppingProduct,
    string IdShopping,
    string IdProduct,
    string IdBusiness,
    double Quantity,
    double Price,
    double TotalPrice,
    string ProductName,
    int IdCompartment,
    DateOnly Date
    );
