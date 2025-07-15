using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.DashboardPowerBI.EdsView.Entities;

public class EdsViewEntity
{
    [Key]
    public int IdEds { get; set; } = default!;
    public int IdProduct { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Nit { get; set; } = default!;
    public string? Address { get; set; } = default!;
    public string Sicom { get; set; } = default!;
    public DateOnly Date { get; set; } = default!;
    public int IdBusiness { get; set; } = default!;

}