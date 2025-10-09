using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Application.Account.Dtos;

public class AccountDto : AuditableEntity
{
    public int IdAccount { get; set; }
    public string AccountType { get; set; } = null!;
    public string Bank { get; set; } = null!;
    public string Account { get; set; } = null!;
    public string Holder { get; set; } = null!;
}
