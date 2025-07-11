using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.EdsTank.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.EdsTank.DomainEdsTank;
using Poliedro.Eds.Domain.EdsTank.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EdsTank.DomainEdsTank.Impl;

public class EdsTankGetByIdEdsTank(DataBaseContext context, IRedisService redisService) : IEdsTankGetByIdEdsTank
{
    public async Task<Result<EdsTankEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"edsTank:{id}";
        var cachedData = await redisService.GetCacheAsync<EdsTankEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return EdsTankErrorBuilder.EdsTankNotFoundException(id);

        var data = await context.EdsTank
            .FirstAsync(c => c.IdEdsTank == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        return await context.Tank
            .AsNoTracking()
            .AnyAsync(c => c.IdTank == id);
    }

}
