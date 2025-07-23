using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.CourtDispensersInventory.Entities;

public class CourtDispensersInventoryEntity : AuditableEntity
{
    [Key]
    public int IdCourtDispensersInventory { get; set; }
    public int IdCourtdDispensers { get; set; }
    public int IdInventory { get; init; }
}
