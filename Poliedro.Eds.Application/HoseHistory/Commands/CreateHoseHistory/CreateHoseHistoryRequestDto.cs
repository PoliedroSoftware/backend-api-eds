namespace Poliedro.Eds.Application.HoseHistory.Commands.CreateHoseHistory;

public record CreateHoseHistoryRequestDto(
    int IdHose,
    int IdDispensers,
    double AccumulatedGallons,
    double AccumulatedAmount,
    DateTime Date);


