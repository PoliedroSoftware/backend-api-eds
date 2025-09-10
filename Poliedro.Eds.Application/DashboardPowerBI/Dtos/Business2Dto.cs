namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos;

public record Business2Dto
(
    string IdBusiness,
    string BusinessName,
    string EdsName,
    string TankNumber,
    string CompartimentNumber,
    string IdProduct,
    string ProductName,
    DateOnly ProductDate
);
