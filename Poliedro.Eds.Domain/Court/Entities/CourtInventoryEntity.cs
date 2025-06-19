namespace Poliedro.Eds.Domain.Court.Entities;

public class CourtInventoryEntity
{
    public int IdInventory { get; set; }
    public DateOnly Date { get; set; }
    public string ReferenceType { get; set; }
    public int ReferenceId { get; set; }
}
