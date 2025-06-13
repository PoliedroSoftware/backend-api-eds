namespace Poliedro.Eds.Application.Court.Commands.CreateCourt;

public record CourtDispenserCommand
    (
        double AccumulatedAmount,
        double AccumulatedGallons,
        double LastAccumulatedAmount,
        double LastAccumulatedGallons,
        double AmountDifferenceResult,
        double GallonsDifferenceResult,
        int IdHose,
        int NumberName,
        int DispenserNumber
    );
