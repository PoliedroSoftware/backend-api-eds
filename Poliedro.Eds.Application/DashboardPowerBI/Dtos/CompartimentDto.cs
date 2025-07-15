namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos;

    public record CompartimentDto(
    string IdCompartment,
    string IdProduct,
    string IdBusiness,
    int Number,
    double Nominal,
    double Operative,
    double Stock,
    double Height,
    string IdTank,
    DateOnly Date
    );

