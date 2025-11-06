namespace Poliedro.Eds.Application.Compartiment.Commands.CreateCompartiment;

public record CreateCompartimentRequestDto(
    int Number,
    double Nominal,
    double Operative,
    int IdProduct,
    double Height,
    int IdTank);


