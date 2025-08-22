namespace Poliedro.Eds.Application.Compartiment.Commands.CreateCompartiment;

public record CreateCompartimentRequestDto(
    int Number,
    double Nominal,
    double Operative,
    double Height,
    int IdTank,
    int IdProduct
);


