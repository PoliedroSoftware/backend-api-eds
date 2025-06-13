using Poliedro.Eds.Domain.TypeOfCollection.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Poliedro.Eds.Domain.Court.Entities;

public class CourtTypeOfCollectionEntity
{
    public int IdCourtTypeOfCollection { get; set; }
    public int IdCourt { get; set; }
    public int IdTypeOfCollection { get; set; }
    public double Amount { get; set; }
    [NotMapped]
    public string TypeOfCollectionName { get; set; }
    public string Description { get; set; }
    public TypeOfCollectionEntity TypeOfCollection { get; set; }
}
