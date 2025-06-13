using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.EdsTank.Entities;

public class EdsTankEntity
{
    [Key]
    public int IdEdsTank { get; set; } = default!;
    public int IdEds { get; set; } = default!;
    public int IdTank { get; init; }
}
