namespace Poliedro.Eds.Application.Compartiment.Commands.CreateCompartiment;

public record CreateCompartimentRequestDto(
    int Number,
    decimal Nominal,
    decimal Operative,
    int IdProduct,
    decimal Height,
    int IdTank);


