using System.ComponentModel.DataAnnotations.Schema;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.Compartiment.Entities;

public class CompartimentEntity : AuditableEntity
{
    public int IdCompartiment { get; set; }
    public int Number { get; set; }
    public decimal Nominal { get; set; }
    public decimal Operative { get; set; }
    public decimal Height { get; set; }
    public int IdProduct { get; set; }
    public int IdTank { get; set; }
    public DateTime Date { get; set; }
    [NotMapped]
    public string? NumberTank { get; set; }
    [NotMapped]
    public string? NameProduct { get; set; }
}



