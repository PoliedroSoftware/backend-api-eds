namespace Poliedro.Eds.Application.HoseHistory.Dtos;

public record HoseHistoryDto(
   int IdHoseHistory,
   int IdHose,
   int IdDispensers,
   double AccumulatedGallons,
   double AccumulatedAmount,
   DateTime Date);