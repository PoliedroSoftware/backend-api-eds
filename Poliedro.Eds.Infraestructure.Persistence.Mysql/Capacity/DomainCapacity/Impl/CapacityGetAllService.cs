using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Capacity.DomainCapacity.Impl;

public class CapacityGetAllService(
    ITenantDbContextFactory dbContextFactory,
    ILogger<CapacityGetAllService> logger
    ) : ICapacityGetAllService
{
    public async Task<IEnumerable<CapacityEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        logger.LogInformation("Getting all capacities with pagination - Page: {PageNumber}, PageSize: {PageSize}", 
            paginationParams.PageNumber, paginationParams.PageSize);
        
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.Capacity
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        
        logger.LogInformation("Successfully retrieved {Count} capacities", data.Count);
        
        return data;
    }
}
