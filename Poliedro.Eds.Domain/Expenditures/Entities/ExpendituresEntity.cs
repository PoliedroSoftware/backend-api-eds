using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.Expenditures.Entities;

public class ExpendituresEntity : AuditableEntity
{
    [Key]
    public int IdExpenditures { get; set; } = default!;
    public string Description { get; set; } = default!;
}
