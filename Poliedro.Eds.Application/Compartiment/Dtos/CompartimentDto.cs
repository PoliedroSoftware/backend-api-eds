
namespace Poliedro.Eds.Application.Compartiment.Dtos;

public record CompartimentDto(
    int IdCompartiment,
    int Number,
    decimal Nominal,
    decimal Operative,
    int IdProduct,
    decimal Height,
    int IdTank,
    string? NumberTank,
    string? NameProduct
    );
