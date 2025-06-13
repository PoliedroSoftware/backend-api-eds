using Poliedro.Eds.Domain.Dispensers.Entities;
using Poliedro.Eds.Domain.ProductType.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Poliedro.Eds.Domain.Hose.Entities;

public class HoseEntity
{
    [Key]
    public int IdHose { get; set; }
    public int Number { get; set; }
    public int IdDispensers { get; set; }
    public double AccumulatedGallons { get; set; }
    public double AccumulatedAmount { get; set; }
    public int IdProductType { get; set; }
}
