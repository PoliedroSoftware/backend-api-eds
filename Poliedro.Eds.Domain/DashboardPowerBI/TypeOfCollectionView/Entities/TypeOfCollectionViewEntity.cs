using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.TypeOfCollection.Entities;

public class TypeOfCollectionViewEntity
{
    [Key]
    public int IdTypeOfCollection { get; set; } = default!;
    public int IdProduct { get; set; } = default!;
    public int IdBusiness { get; set; } = default!;
    public DateOnly Date { get; set; } = default!;
    public string Description { get; set; } = default!;
}