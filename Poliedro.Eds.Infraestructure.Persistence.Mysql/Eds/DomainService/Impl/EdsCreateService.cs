using Poliedro.Eds.Application.Eds.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;
using Microsoft.EntityFrameworkCore.Internal;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Eds.DomainEds.Impl;

public class EdsCreateService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IEdsCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(EdsEntity EdsEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.Eds.AddAsync(EdsEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return EdsErrorBuilder.EdsCreationException();
        await redisService.RemoveByPrefixAsync("eds:");
        return VoidResult.Instance;
    }
}