using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.CompartimentCapacity.Entities;

public class CompartimentCapacityEntity
{
    [Key]
    public int IdCompartimentCapacity { get; set; } = default!;
    public int IdCompartiment { get; set; } = default!;
    public int IdCapacity { get; set; } = default!;
    public byte Default { get; set; } = default!;
}