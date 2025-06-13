namespace Poliedro.Eds.Domain.Court.Dto;

public record CourtDispenserDto(
    int IdCourtDispensers,
    int IdCourt,
    double AccumulatedAmount,
    double AccumulatedGallons,
    int IdProduct,
    int IdCompartiment,
    int IdHose);


