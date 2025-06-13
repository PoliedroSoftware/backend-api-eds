namespace Poliedro.Eds.Domain.Court.Dto;

public record CourtTypeOfCollectionDto(
    int IdCourtTypeOfCollection,
    int IdCourt,
    int IdTypeOfCollection,
    double Amount,
    string TypeOfCollectionName,
    string Description,
    TypeOfCollectionDto TypeOfCollection);


