using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.Expenditures.Entities;

public class ExpendituresEntity
{
    [Key]
    public int IdExpenditures { get; set; } = default!;
    public string Description { get; set; } = default!;
}
