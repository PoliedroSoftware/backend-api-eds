using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Phone.DomainServices.GetAll;
using Poliedro.Eds.Domain.Phone.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Phone.DomainService.Impl;

public class PhoneGetAllService(ITenantDbContextFactory dbContextFactory) : IPhoneGetAllService
{
    public async Task<IEnumerable<PhoneEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var totalRows = await context.Phone.CountAsync();
        var data = await context.Phone
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return data;
    }
}
