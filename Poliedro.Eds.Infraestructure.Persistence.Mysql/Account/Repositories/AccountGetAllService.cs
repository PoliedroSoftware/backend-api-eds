using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Account.Entities;
using Poliedro.Eds.Domain.Account.Services;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Account.Repositories;

public class AccountGetAllService(ITenantDbContextFactory dbContextFactory) : IAccountGetAllService
{
    public async Task<IEnumerable<AccountEntity>> GetAllAsync()
    {
        using var db = dbContextFactory.CreateDbContext();
        return await db.Set<AccountEntity>().ToListAsync();
    }
}
