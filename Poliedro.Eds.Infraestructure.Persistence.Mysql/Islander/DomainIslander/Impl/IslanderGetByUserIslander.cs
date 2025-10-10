using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Islander.Domainislander.Impl;

public class IslanderGetByUserIslander(ITenantDbContextFactory dbContextFactory) : IIslanderGetByUserIslander
{
    public async Task<Result<IslanderEntity?, Error>> GetByUserAsync(string name)
    {
        using var context = dbContextFactory.CreateDbContext();
        var entity = await context.Islander
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Name == name);
        return Result<IslanderEntity?, Error>.Success(entity);
    }

    public async Task<bool> ExistsAsync(string name)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Islander
            .AsNoTracking()
            .AnyAsync(i => i.Name == name);
    }
}
