using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.TypeOfCollection.Entities;

public class TypeOfCollectionEntity
{
    [Key]
    public int IdTypeOfCollection { get; set; } = default!;
    public string Description { get; set; } = default!;
}