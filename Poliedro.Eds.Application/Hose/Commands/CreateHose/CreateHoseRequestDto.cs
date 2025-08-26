namespace Poliedro.Eds.Application.Hose.Commands.CreateHose;

public record CreateHoseRequestDto(
    int IdDispensers,
    int Number,    
    double AccumulatedAmount,
    double AccumulatedGallons,
    int IdProductType,
    int IdCompartiment);


