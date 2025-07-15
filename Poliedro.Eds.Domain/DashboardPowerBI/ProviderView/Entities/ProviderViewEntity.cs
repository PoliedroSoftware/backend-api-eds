using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.DashboardPowerBI.ProviderView.Entities;

public class ProviderViewEntity
{
    [Key]
    public int IdProvider { get; set; }
    public int? IdBusiness { get; set; }
    public int IdProduct { get; set; }
    public string? Name { get; set; }
    public DateOnly Date { get; set; }
}