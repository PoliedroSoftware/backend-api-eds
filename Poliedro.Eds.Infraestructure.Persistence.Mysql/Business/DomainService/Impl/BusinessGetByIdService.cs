using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Business.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;

public class BusinessGetByIdService(DataBaseContext context,IRedisService redisService) : IBusinessGetByIdService

   {
    public async Task<Result<BusinessEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"business:{id}";
        var cachedData = await redisService.GetCacheAsync<BusinessEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return BusinessErrorBuilder.BusinessNotFoundException(id);

        var data = await context.Business
            .FirstAsync(c => c.IdBusiness == id);

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