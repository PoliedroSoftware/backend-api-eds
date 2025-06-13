
namespace Poliedro.Eds.Application.Compartiment.Dtos;

public record CompartimentDto(
    int IdCompartment,
    int Number,
    double Nominal,
    double Operative,
    double Stock,
    double Height,
    int IdTank
    );
