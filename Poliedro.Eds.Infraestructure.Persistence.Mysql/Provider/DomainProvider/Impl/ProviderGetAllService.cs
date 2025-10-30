using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Provider.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Provider.DomainProvider.Impl;

public class ProviderGetAllService(ITenantDbContextFactory dbContextFactory, ILogger<ProviderGetAllService> logger) : IProviderGetAllService
{
    public async Task<IEnumerable<ProviderEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        logger.LogInformation("Getting all providers with pagination - Page: {PageNumber}, PageSize: {PageSize}", 
            paginationParams.PageNumber, paginationParams.PageSize);
        
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.Provider
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        
        logger.LogInformation("Successfully retrieved {Count} providers", data.Count());
        
        return data;
    }
}
