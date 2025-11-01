using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.TankHistory.Entities;

public class TankHistoryEntity : AuditableEntity
{
    [Key]
    public int IdTankHistory { get; set; }
    public int IdTank { get; set; }
    public string Number { get; set; } = default!;
    public int Compartment { get; set; }
    public double Ability { get; set; }
    public double? Stock { get; set; }
    public DateTime Date { get; set; }
}
