namespace Poliedro.Eds.Application.Account.Dtos;

public class AccountCreateDto
{
    public string AccountType { get; set; } = null!;
    public string Bank { get; set; } = null!;
    public string Account { get; set; } = null!;
    public string Holder { get; set; } = null!;
}
