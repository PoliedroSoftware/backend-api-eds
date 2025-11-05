using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.TankHistory.Entities;

public class TankHistoryEntity : AuditableEntity
{
    [Key]
    public int IdTankHistory { get; set; }
    public int IdTank { get; set; }
    public double? OldAbility { get; set; }
    public double? NewAbility { get; set; }
}
