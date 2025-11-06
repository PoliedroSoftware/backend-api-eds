namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos;

public record ShoppingDto
(
    string IdShopping,
    string Invoice,
    DateTime Date,
    string IdProvider,
    string IdCategory,
    double Amount
    );
