namespace Poliedro.Eds.Application.Court.Commands.CreateCourt;

public record CourtExpenditureCommand(
    int IdExpenditures,
    double Amount,
    string? Description,
    string ExpenditureName
    );