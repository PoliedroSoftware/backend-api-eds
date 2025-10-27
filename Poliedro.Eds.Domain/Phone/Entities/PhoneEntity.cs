using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.Phone.Entities;

public class PhoneEntity : AuditableEntity
{
    public int IdPhone { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
