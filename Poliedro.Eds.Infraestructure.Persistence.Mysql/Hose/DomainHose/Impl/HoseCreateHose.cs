using Poliedro.Eds.Application.Hose.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Hose.DomainHose.Impl;

public class HoseCreateHose(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IHoseCreateHose
{
    public async Task<Result<VoidResult, Error>> CreateAsync(HoseEntity hoseEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        var dispenser = await context.Dispensers
            .Where(d => d.Id == hoseEntity.IdDispensers)
            .Select(d => new { d.HoseNumber })
            .FirstOrDefaultAsync();

        if (dispenser == null)
            return HoseErrorBuilder.HoseCreationException();

        int hoseCount = await context.Hose.CountAsync(h => h.IdDispensers == hoseEntity.IdDispensers);

        if (hoseCount >= dispenser.HoseNumber)
            return HoseErrorBuilder.HoseCreationException();

            await context.Hose.AddAsync(hoseEntity);
            var result = await context.SaveChangesAsync() > 0;

            if (!result)
                return HoseErrorBuilder.HoseCreationException();

            await redisService.RemoveByPrefixAsync("hose:");

            return VoidResult.Instance;
       
    }
}
