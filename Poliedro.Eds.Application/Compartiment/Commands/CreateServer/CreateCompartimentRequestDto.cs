namespace Poliedro.Eds.Application.Compartiment.Commands.CreateCompartiment;

public record CreateCompartimentRequestDto(
    int Number,
    double Nominal,
    double Operative,
    double Stock,
    double Height,
    int IdTank);


