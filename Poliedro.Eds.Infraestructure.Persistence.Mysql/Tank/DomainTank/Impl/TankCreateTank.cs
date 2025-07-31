using Poliedro.Eds.Application.Tank.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Tank.DomainTank;
using Poliedro.Eds.Domain.Tank.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Tank.DomainTank.Impl;

public class TankCreateTank(ITenantDbContextFactory dbContextFactory) : ITankCreateTank
{
    public async Task<Result<VoidResult, Error>> CreateAsync(TankEntity tankEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.Tank.AddAsync(tankEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return TankErrorBuilder.TankCreationException();
        return VoidResult.Instance;
    }
}
