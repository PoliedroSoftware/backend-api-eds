namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos;

public record CompartimentDto(
string IdCompartment,
int Number,
double Nominal,
double Operative,
double Stock,
double Height,
string IdProduct,
string ProductName,
string IdTank,
string IdEds,
string IdBusiness,
DateOnly Date
);

