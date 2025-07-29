using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.CompartimentCapacity.Entities;

public class CompartimentCapacityEntity : AuditableEntity
{
    [Key]
    public int IdCompartimentCapacity { get; set; } = default!;
    public int IdCompartiment { get; set; } = default!;
    public int IdCapacity { get; set; } = default!;
    public byte Default { get; set; } = default!;
}
