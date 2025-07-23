using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.HoseHistory.Entities;

public class HoseHistoryEntity : AuditableEntity
{
    [Key]
    public int IdHoseHistory { get; set; }
    public int IdHose { get; set; }
    public int IdDispensers { get; init; }
    public double AccumulatedGallons { get; init; }
    public double AccumulatedAmount { get; init; }
    public DateTime Date { get; init; }

}
