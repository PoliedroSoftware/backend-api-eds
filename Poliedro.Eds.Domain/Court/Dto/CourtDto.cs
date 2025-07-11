namespace Poliedro.Eds.Domain.Court.Dto;

public record CourtDto(
    int IdCourt,
    int IdIslander,
    DateOnly DateStarttime,
    TimeOnly Starttime,
    DateOnly DateEndtime,
    TimeOnly Endtime,
    int Consecutive,
    int IdEds,
    string? Descripcion,
    double Distintic,
    IEnumerable<CourtDispenserDto> CourtDispensers,
    IEnumerable<DocumentDto> CourtDocuments,
    IEnumerable<CourtExpenditureDto> CourtExpenditures,
    IEnumerable<CourtTypeOfCollectionDto> CourtTypeOfCollections);

