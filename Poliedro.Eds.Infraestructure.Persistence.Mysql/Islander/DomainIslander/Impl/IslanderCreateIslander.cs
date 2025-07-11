using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Islander.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Islander.Domainislander.Impl;
public class IslanderCreateIslander(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IIslanderCreateIslander
{
    public async Task<Result<VoidResult, Error>> CreateAsync(IslanderEntity islanderEntity)
    {
        using var context = dbContextFactory.CreateDbContext();

        await context.Islander.AddAsync(islanderEntity);

        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return IslanderErrorBuilder.IslanderCreationException();
        await redisService.RemoveByPrefixAsync("islander:");

        return VoidResult.Instance;
    }
}