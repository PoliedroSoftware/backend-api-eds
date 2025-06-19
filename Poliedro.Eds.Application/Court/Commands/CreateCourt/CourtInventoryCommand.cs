namespace Poliedro.Eds.Application.Court.Commands.CreateCourt;

public record CourtInventoryCommand
(
    DateOnly Date,
    string ReferenceType,
    int ReferenceId
);
