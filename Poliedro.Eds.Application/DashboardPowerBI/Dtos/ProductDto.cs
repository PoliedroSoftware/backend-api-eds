namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos;

public record ProductDto
(
    string IdProduct,
    string IdBusiness,
    string Name,
    string IdProductType,
    double Price,
    DateOnly Date
    );
