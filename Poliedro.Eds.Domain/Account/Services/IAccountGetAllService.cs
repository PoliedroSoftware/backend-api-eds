using Poliedro.Eds.Domain.Account.Entities;

namespace Poliedro.Eds.Domain.Account.Services;

public interface IAccountGetAllService
{
    Task<IEnumerable<AccountEntity>> GetAllAsync();
}
