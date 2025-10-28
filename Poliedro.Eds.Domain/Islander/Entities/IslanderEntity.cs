using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.Islander.Entities;

public class IslanderEntity : AuditableEntity
{
    [Key]
    public int IdIslander { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int IdEds { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    [NotMapped]
    public string? NameEDS { get; set; }

}
