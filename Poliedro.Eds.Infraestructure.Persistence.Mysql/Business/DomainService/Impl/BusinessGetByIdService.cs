using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Business.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;

public class BusinessGetByIdService(
    ITenantDbContextFactory dbContextFactory, 
    IRedisService redisService,
    ILogger<BusinessGetByIdService> logger) : IBusinessGetByIdService

{
    public async Task<Result<BusinessEntity, Error>> GetByIdAsync(int id)
    {
        logger.LogInformation("Getting business by ID: {BusinessId}", id);

        using var context = dbContextFactory.CreateDbContext();

        string cacheKey = $"business:{id}";
        var cachedData = await redisService.GetCacheAsync<BusinessEntity>(cacheKey);

        if (cachedData is not null)
        {
            logger.LogInformation("Business found in cache: {BusinessId}", id);
            return cachedData;
        }

        if (!await EntityExists(id))
        {
            logger.LogWarning("Business not found: {BusinessId}", id);
            return BusinessErrorBuilder.BusinessNotFoundException(id);
        }

        var data = await context.Business
            .FirstAsync(c => c.IdBusiness == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));
        
        logger.LogInformation("Successfully retrieved business: {BusinessId} - {BusinessName}", id, data.Name);

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();

        return await context.Business
            .AsNoTracking()
            .AnyAsync(c => c.IdBusiness == id);
    }
}
