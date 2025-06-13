
namespace Poliedro.Eds.Application.Shopping.Dtos;

public record ShoppingDto(
    int IdShopping,
    string Invoice,
    DateTime Date, 
    int IdProvider,
    int IdCategory,
    double Amount
    );
