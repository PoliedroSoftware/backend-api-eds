namespace Poliedro.Eds.Application.Court.Commands.CreateCourt;

public record DocumentCommand(
    string? Descripcion,
    string? DocumentName
    );