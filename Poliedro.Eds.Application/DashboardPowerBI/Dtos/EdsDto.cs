namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos;

public record EdsDto
(
    string IdEds,
    string EdsName,
    string Nit,
    string Address,
    string Sicom,
    DateOnly Date,
    string IdBusiness,
    string BusinessName,
    string IdProduct,
    string ProductName
    );
