using Poliedro.Eds.Domain.Audit.Entities;
using Poliedro.Eds.Domain.Inventory.Entities;

namespace Poliedro.Eds.Domain.Court.Entities;

public class CourtEntity : AuditableEntity
{
    public int IdCourt { get; set; }
    public int IdIslander { get; set; }
    public DateOnly DateStarttime { get; set; }
    public TimeOnly Starttime { get; set; }
    public DateOnly DateEndtime { get; set; }
    public TimeOnly Endtime { get; set; }
    public int Consecutive { get; set; }
    public int IdEds { get; set; }
    public string? Descripcion { get; set; }
    public double Distintic { get; set; }
    public IEnumerable<CourtDispenserEntity> CourtDispensers { get; set; }
    public IEnumerable<DocumentEntity?>? CourtDocuments { get; set; }
    public IEnumerable<CourtExpenditureEntity?>? CourtExpenditures { get; set; }
    public IEnumerable<CourtTypeOfCollectionEntity> CourtTypeOfCollections { get; set; }
    public InventoryEntity CourtInventory { get; set; }
}
