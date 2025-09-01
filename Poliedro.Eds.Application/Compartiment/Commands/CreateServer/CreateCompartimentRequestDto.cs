namespace Poliedro.Eds.Application.Compartiment.Commands.CreateCompartiment;

public record CreateCompartimentRequestDto(
    int Number,
    double Nominal,
    double Operative,
<<<<<<< HEAD
    int IdProduct,
=======
    double? Stock,
>>>>>>> New-service-StrongBox
    double Height,
    int IdTank);


