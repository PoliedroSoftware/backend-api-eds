namespace Poliedro.Eds.Domain.DashboardPowerBI.CompartimentView.Entities;

public class CompartimentViewEntity
{
    public int IdCompartment { get; set; } // id_compartiment
    public string IdProduct { get; set; }
    public string IdBusiness { get; set; }
    public int Number { get; set; } // number
    public double Nominal { get; set; } // nominal
    public double Operative { get; set; } // operative
    public double Stock { get; set; } // stock
    public double Height { get; set; } // height
    public int IdTank { get; set; } // id_tank
    public DateOnly Date { get; set; }
}


