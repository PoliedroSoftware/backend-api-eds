using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.Capacity.Entities;

public class CapacityEntity : AuditableEntity
{
    [Key]
    public int IdCapacity { get; set; } = default!;
    public string Code { get; set; } = default!;
    public double Height { get; set; } = default!;
    public double Gallon { get; set; } = default!;
    public int Liters { get; set; } = default!;
}
