using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Business.Commands.UpdateBusiness;
using Poliedro.Eds.Application.Business.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;

public class BusinessUpdateService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IBusinessUpdateService
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(BusinessEntity businessEntity)
    {
        if (!await EntityExists(businessEntity.IdBusiness))
            return BusinessErrorBuilder.BusinessNotFoundException(businessEntity.IdBusiness);

        using var context = dbContextFactory.CreateDbContext();
        context.Business.Update(businessEntity);

        if (await context.SaveChangesAsync() <= 0)
            return BusinessErrorBuilder.BusinessUpdateException();

        await redisService.RemoveByPrefixAsync("business:");
        return VoidResult.Instance;
    }

    public async Task<BusinessEntity?> GetByIdAsync(int id)
    {
        using var context = dbContextFactory.CreateDbContext();

        return await context.Business
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.IdBusiness == id);
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();

        return await context.Business
            .AsNoTracking()
            .AnyAsync(c => c.IdBusiness == id);
    }

}
