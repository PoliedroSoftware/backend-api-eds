using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.TypeOfCollection.Entities;

public class TypeOfCollectionEntity : AuditableEntity
{
    [Key]
    public int IdTypeOfCollection { get; set; } = default!;
    public string Description { get; set; } = default!;
}