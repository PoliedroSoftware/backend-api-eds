using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Phone.DomainServices.GetAll;
using Poliedro.Eds.Domain.Phone.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Phone.DomainService.Impl;

public class PhoneGetAllService(
    ITenantDbContextFactory dbContextFactory,
    ILogger<PhoneGetAllService> logger) : IPhoneGetAllService
{
    public async Task<IEnumerable<PhoneEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        logger.LogInformation("Getting all phones with pagination - Page: {PageNumber}, PageSize: {PageSize}", 
            paginationParams.PageNumber, paginationParams.PageSize);
        
        using var context = dbContextFactory.CreateDbContext();
        var totalRows = await context.Phone.CountAsync();
        var data = await context.Phone
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        
        logger.LogInformation("Successfully retrieved {Count} phones out of {Total} total", 
            data.Count, totalRows);
        
        return data;
    }
}
