namespace Poliedro.Eds.Application.Hose.Commands.CreateHose;

public record CreateHoseRequestDto(
    int Number,
    int IdDispensers,
    double AccumulatedGallons,
    double AccumulatedAmount,
    int IdProductType);


