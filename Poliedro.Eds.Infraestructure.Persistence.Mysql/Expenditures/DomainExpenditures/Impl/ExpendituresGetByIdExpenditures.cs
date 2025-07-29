using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Expenditures.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Expenditures.DomainExpenditures;
using Poliedro.Eds.Domain.Expenditures.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Expenditures.DomainExpenditures.Impl;

public class ExpendituresGetByIdExpenditures(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IExpendituresGetByIdExpenditures
{
    public async Task<Result<ExpendituresEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"expenditures:{id}";
        var cachedData = await redisService.GetCacheAsync<ExpendituresEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return ExpendituresErrorBuilder.ExpendituresNotFoundException(id);

        using var context = dbContextFactory.CreateDbContext();
        var data = await context.Expenditures
            .FirstAsync(c => c.IdExpenditures == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Tank
            .AsNoTracking()
            .AnyAsync(c => c.IdTank == id);
    }

}
