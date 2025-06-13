using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.Islander.Entities;

public class IslanderEntity
{
    [Key]
    public int IdIslander { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int IdEds { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

}
