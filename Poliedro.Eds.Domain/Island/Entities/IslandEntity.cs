using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.Island.Entities;

public class IslandEntity : AuditableEntity
{
    [Key]
    public int IdIsland { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int? IdEds { get; set; } = default;

    [NotMapped]
    public string? NameEDS { get; set; }
}
