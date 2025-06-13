using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.Eds.Entities;

public class EdsEntity
{
    [Key]
    public int IdEds { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Nit { get; set; } = default!;
    public string? Address { get; set; } = default!;
    public string Sicom { get; set; } = default!;
    public int IdBusiness { get; set; } = default!;

}