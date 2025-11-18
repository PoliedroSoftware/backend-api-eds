namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos.BusinessDashboardView;

public record BusinessDashboardViewDto
(
    string IdBusiness,
    string BusinessName,
    string EdsName,
    string TankNumber,
    string CompartimentNumber,
    string IdProduct,
    string ProductName,
    string ProductDate
);
