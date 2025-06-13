using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Expenditures.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Expenditures.DomainExpenditures;
using Poliedro.Eds.Domain.Expenditures.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Expenditures.DomainExpenditures.Impl;

public class ExpendituresGetAllExpenditures(DataBaseContext context,IRedisService redisService) : IExpendituresGetAllExpenditures
{

    public async Task<IEnumerable<ExpendituresEntity>> GetAllAsync(PaginationParams paginationParams)
    {
         var totalRows = await context.Expenditures.CountAsync();

       string cacheKey = $"expenditures:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<ExpendituresEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;
        
        var data = await context.Expenditures
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

       
        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}
