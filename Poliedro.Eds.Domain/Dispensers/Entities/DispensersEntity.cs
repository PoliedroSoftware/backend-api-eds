using System.ComponentModel.DataAnnotations.Schema;
using Poliedro.Eds.Domain.Audit.Entities;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Domain.Hose.Entities;

namespace Poliedro.Eds.Domain.Dispensers.Entities;

public class DispensersEntity : AuditableEntity
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public int Number { get; set; }
    public int DispenserTypeId { get; set; }
    public int EdsId { get; set; }
    public int IdIsland { get; set; }
    public int HoseNumber { get; set; }

    public virtual EdsEntity? Eds { get; set; }
}


