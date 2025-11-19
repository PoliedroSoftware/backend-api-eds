using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.DashboardPowerBI.CompartimentDashboardView.Entities;

public class CompartimentDashboardViewEntity
{
    [Key]
    public int IdCompartment { get; set; }
    public int Number { get; set; }
    public int Nominal { get; set; }
    public int Operative { get; set; }
    public double Height { get; set; }
    public int IdProduct { get; set; }
    public string ProductName { get; set; } = default!;
    public int IdTank { get; set; }
    public int IdEds { get; set; }
    public int IdBusiness { get; set; }
    public DateOnly Date { get; set; }
}
