using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.EdsTank.Entities;

public class EdsTankEntity : AuditableEntity
{
    [Key]
    public int IdEdsTank { get; set; } = default!;
    public int IdEds { get; set; } = default!;
    public int IdTank { get; init; }
}
