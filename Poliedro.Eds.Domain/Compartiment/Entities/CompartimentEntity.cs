using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.Compartiment.Entities;

public class CompartimentEntity : AuditableEntity
{
    public int IdCompartiment { get; set; }
    public int Number { get; set; }
    public double Nominal { get; set; }
    public double Operative { get; set; }
    public double Height { get; set; }
    public int IdProduct { get; set; }
    public int IdTank { get; set; }
    public DateTime Date { get; set; }
    public string? NumberTank { get; set; }
}



