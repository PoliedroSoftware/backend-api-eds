using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.DashboardPowerBI.CompartimentView.Entities;

public class CompartimentViewEntity
{
    [Key]
    public int IdCompartment { get; set; }
    public int Number { get; set; }
    public double Nominal { get; set; }
    public double Operative { get; set; }
    public double Height { get; set; }
    public int IdProduct { get; set; }
    public string ProductName { get; set; }
    public int IdTank { get; set; }
    public int IdEds { get; set; }
    public int? IdBusiness { get; set; }    
    public DateOnly Date { get; set; }
}
