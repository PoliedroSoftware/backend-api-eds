using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.DispenserType.Entities;

public class DispenserTypeEntity : AuditableEntity
{
    public int IdType { get; set; }
    public string Description { get; set; } = default!;
}