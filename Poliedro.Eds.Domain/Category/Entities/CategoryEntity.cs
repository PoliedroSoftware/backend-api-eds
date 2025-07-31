using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.Category.Entities;

public class CategoryEntity : AuditableEntity
{
    public int IdCategory { get; set; }
    public string Description { get; set; } = default!;
}


