using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.Account.Entities;

public class AccountEntity : AuditableEntity
{
    [Key]
    public int IdAccount { get; set; }
    public string AccountType { get; set; } = null!;
    public string Bank { get; set; } = null!;
    public string Account { get; set; } = null!;
    public string Holder { get; set; } = null!;
}
