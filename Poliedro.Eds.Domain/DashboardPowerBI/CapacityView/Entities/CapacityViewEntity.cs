using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.DashboardPowerBI.CapacityView.Entities;

public class CapacityViewEntity
{
    [Key]
    public int IdCapacity { get; set; } = default!;
    public int IdBusiness { get; set; } = default!;
    public int IdProduct { get; set; } = default!;
    public string Code { get; set; } = default!;
    public double Height { get; set; } = default!;
    public double Gallon { get; set; } = default!;
    public int Liters { get; set; } = default!;
    public DateOnly Date { get; set; } = default!;
}
