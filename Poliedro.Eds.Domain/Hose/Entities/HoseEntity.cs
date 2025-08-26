using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;
using Poliedro.Eds.Domain.Dispensers.Entities;
using Poliedro.Eds.Domain.ProductType.Entities;

namespace Poliedro.Eds.Domain.Hose.Entities;

public class HoseEntity : AuditableEntity
{
    [Key]
    public int IdHose { get; set; }
    public int IdDispensers { get; set; }
    public int Number { get; set; }    
    public double AccumulatedAmount { get; set; }
    public double AccumulatedGallons { get; set; }
    public int IdProductType { get; set; }
    public int IdCompartiment { get; set; }

    
    public virtual DispensersEntity? Dispenser { get; set; }
    public virtual ProductTypeEntity? ProductType { get; set; }
}
