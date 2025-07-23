using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.Hose.Entities;

public class HoseEntity : AuditableEntity
{
    [Key]
    public int IdHose { get; set; }
    public int Number { get; set; }
    public int IdDispensers { get; set; }
    public double AccumulatedGallons { get; set; }
    public double AccumulatedAmount { get; set; }
    public int IdProductType { get; set; }
}
