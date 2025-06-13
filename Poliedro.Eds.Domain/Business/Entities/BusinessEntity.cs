using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.Business.Entities;

public class BusinessEntity
{
    [Key]
    public int IdBusiness { get; set; } = default!;
    public string Name { get; set; } = default!;
}