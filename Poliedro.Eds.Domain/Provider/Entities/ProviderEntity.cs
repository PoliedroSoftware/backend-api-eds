using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.Provider.Entities;

public class ProviderEntity : AuditableEntity
{
    [Key]
    public int IdProvider { get; set; } = default!;
    public string Name { get; set; } = default!;
}
