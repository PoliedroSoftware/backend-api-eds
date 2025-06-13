using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.CourtDispensersInventory.Entities;

public class CourtDispensersInventoryEntity
{
    [Key]
    public int IdCourtDispensersInventory { get; set; }
    public int IdCourtdDispensers { get; set; }
    public int IdInventory { get; init; }
}
