
namespace Poliedro.Eds.Application.Compartiment.Dtos;

public record CompartimentDto(
    int IdCompartiment,
    int Number,
    double Nominal,
    double Operative,
    int IdProduct,
    double Height,
    int IdTank,
    string? NumberTank,
    string? NameProduct
    );
