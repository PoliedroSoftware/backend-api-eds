namespace Poliedro.Eds.Application.Court.Commands.CreateCourt;

public record DispenserCommand(
    double AccomulatedAmount,
    double AccomulatedGallons,
    int IdProdut,
    int IdCompartiment,
    int IdHose
    );
