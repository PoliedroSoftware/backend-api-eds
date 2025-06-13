using System.ComponentModel.DataAnnotations.Schema;

namespace Poliedro.Eds.Domain.Court.Entities;

public class CourtExpenditureEntity
{
    public int IdCourtExpenditure { get; set; }
    public int IdCourt { get; set; }
    public int IdExpenditures { get; set; }
    [NotMapped]
    public string ExpenditureName { get; set; }
    public double Amount { get; set; }
    public string? Description { get; set; }
}
