
namespace Poliedro.Eds.Application.Dispensers.Dtos;

public record DispensersDto(
    int Id,
    string Code,
    int Number, 
    int DispenserTypeId,
    int EdsId,
    int IdIsland,
    int HoseNumber
    );
