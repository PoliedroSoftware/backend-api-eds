using Poliedro.Eds.Application.Hose.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Hose.DomainHose.Impl;

public class HoseCreateHose(ITenantDbContextFactory dbContextFactory) : IHoseCreateHose
{
    public async Task<Result<VoidResult, Error>> CreateAsync(HoseEntity hoseEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.Hose.AddAsync(hoseEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return HoseErrorBuilder.HoseCreationException();
        return VoidResult.Instance;


    }
}
