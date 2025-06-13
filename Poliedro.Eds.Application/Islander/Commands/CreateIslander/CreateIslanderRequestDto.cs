namespace Poliedro.Eds.Application.Islander.Commands.CreateIslander;

public record CreateIslanderRequestDto(
    int IdEds,
    string Name,
    string Password,
    string Email,
    string FirstName,
    string LastName);


