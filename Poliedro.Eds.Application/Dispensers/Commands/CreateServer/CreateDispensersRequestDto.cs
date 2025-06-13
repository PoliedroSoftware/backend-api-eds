namespace Poliedro.Eds.Application.Dispensers.Commands.CreateDispensers;

public record CreateDispensersRequestDto(
    string Code,
    int Number,
    int DispenserTypeId,
    int HoseNumber,
    int EdsId,
    int IdIsland);


