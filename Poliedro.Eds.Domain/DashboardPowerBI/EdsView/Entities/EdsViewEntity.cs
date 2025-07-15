using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.DashboardPowerBI.EdsView.Entities;

public class EdsViewEntity
{
    [Key]
    public int IdEds { get; set; }
    public int IdProduct { get; set; }
    public string? Name { get; set; }
    public string? Nit { get; set; }
    public string? Address { get; set; }
    public string? Sicom { get; set; }
    public DateOnly Date { get; set; }
    public int IdBusiness { get; set; }
}