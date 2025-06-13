using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.Provider.Entities;

public class ProviderEntity
{
    [Key]
    public int IdProvider { get; set; } = default!;
    public string Name { get; set; } = default!;
}