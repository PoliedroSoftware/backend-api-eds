namespace Poliedro.Eds.Domain.Court.Entities;

public class CourtDispenserEntity
{
    public int IdCourtDispensers { get; set; }
    public int IdCourt { get; set; }
    public double AccumulatedAmount { get; set; } 
    public double AccumulatedGallons { get; set; }
    public int IdProduct { get; set; }
    public int IdCompartiment { get; set; }
    public int IdHose { get; set; }

}
