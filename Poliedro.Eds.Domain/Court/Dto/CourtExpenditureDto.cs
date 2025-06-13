namespace Poliedro.Eds.Domain.Court.Dto;

public record CourtExpenditureDto(
    int IdCourtExpenditure,
    int IdCourt,
    int IdExpenditures,
    string ExpenditureName,
    double Amount,
    string? Description);


