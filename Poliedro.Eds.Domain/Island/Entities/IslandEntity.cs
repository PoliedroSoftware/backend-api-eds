using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.Island.Entities;

public class IslandEntity
{
    [Key]
    public int IdIsland { get; set; } = default!;
    public string Description { get; set; } = default!;
}
