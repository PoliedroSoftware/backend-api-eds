using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.Tank.Entities;

public class TankEntity : AuditableEntity
{
    [Key]
    public int IdTank { get; set; }
    public string Number { get; set; }
    public int Compartment { get; init; }
    public double Ability { get; init; }
    [NotMapped]
    public string? NameEDS { get; set; }
}
