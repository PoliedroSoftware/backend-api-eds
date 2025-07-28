namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos;

public record CapacityDto
(
     string IdCapacity,
     string IdProduct,
     string IdBusiness,
     string Code,
     double Height,
     double Gallon,
     int Liters,
     DateOnly Date

);
