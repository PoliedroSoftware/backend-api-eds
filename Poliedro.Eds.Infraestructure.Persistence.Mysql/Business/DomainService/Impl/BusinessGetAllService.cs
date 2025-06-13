using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using System.Text.Json;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;

public class BusinessGetAllService(
    DataBaseContext context, 
    IRedisService redisService, ILogger<BusinessGetAllService> logger) : IBusinessGetAllService
{
    public async Task<IEnumerable<BusinessEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        var totalRows = await context.Business.CountAsync();

       string cacheKey = $"business:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<BusinessEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;
        
        var data = await context.Business
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        var logData = new Dictionary<string, object>
        {
            ["Request"] = "business",
            ["Response"] = JsonSerializer.Serialize(data),
            ["TraceId"] = cacheKey
        };
       
        logger.LogInformation("Response Bussiness: {@LogData}", logData);
       
        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}