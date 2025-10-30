using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;

public class BusinessGetAllService(
    ITenantDbContextFactory dbContextFactory,
    ILogger<BusinessGetAllService> logger
    ) : IBusinessGetAllService
{
    public async Task<IEnumerable<BusinessEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        logger.LogInformation("Getting all businesses with pagination - Page: {PageNumber}, PageSize: {PageSize}", 
            paginationParams.PageNumber, paginationParams.PageSize);
        
        using var context = dbContextFactory.CreateDbContext();
        var totalRows = await context.Business.CountAsync();
        var data = await context.Business
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        
        logger.LogInformation("Successfully retrieved {Count} businesses out of {Total} total", 
            data.Count(), totalRows);
        
        return data;
    }
}
