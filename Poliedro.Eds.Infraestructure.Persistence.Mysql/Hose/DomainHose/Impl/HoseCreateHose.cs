using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Hose.DomainHose.Impl;

public class HoseCreateHose(ITenantDbContextFactory dbContextFactory ) : IHoseCreateHose
{
    public async Task<bool> CreateAsync(HoseEntity hoseEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.Hose.AddAsync(hoseEntity);
        var result = await context.SaveChangesAsync() > 0; 
        return result;


    }
}
