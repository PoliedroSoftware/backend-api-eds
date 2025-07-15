using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.DashboardPowerBI.ProviderView.Entities;

public class ProviderViewEntity
{
    [Key]
    public int IdProvider { get; set; } = default!;
    public int IdBusiness { get; set; } = default!;
    public int IdProduct { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DateOnly Date { get; set; } = default!;
}