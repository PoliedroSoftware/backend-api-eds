using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Business.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;

public class BusinessUpdateService(DataBaseContext context, IRedisService redisService) : IBusinessUpdateService
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(BusinessEntity BusinessEntity)
    {
        if (!await EntityExists(BusinessEntity.IdBusiness))
            return BusinessErrorBuilder.BusinessNotFoundException(BusinessEntity.IdBusiness);

        context.Business.Update(BusinessEntity);

        if (await context.SaveChangesAsync() <= 0)
            return BusinessErrorBuilder.BusinessUpdateException();
        await redisService.RemoveByPrefixAsync("business:");
        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Business
            .AsNoTracking()
            .AnyAsync(c => c.IdBusiness == id);
    }
}