using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Phone.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Phone.DomainServices.Update;
using Poliedro.Eds.Domain.Phone.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Phone.DomainService.Impl;

public class PhoneUpdateService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IPhoneUpdateService
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(PhoneEntity phoneEntity)
    {
        if (!await EntityExists(phoneEntity.IdPhone))
            return PhoneErrorBuilder.PhoneNotFoundException(phoneEntity.IdPhone);

        using var context = dbContextFactory.CreateDbContext();
        context.Phone.Update(phoneEntity);

        if (await context.SaveChangesAsync() <= 0)
            return PhoneErrorBuilder.PhoneUpdateException();
        await redisService.RemoveByPrefixAsync("phone:");
        return VoidResult.Instance;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Phone
            .AsNoTracking()
            .AnyAsync(c => c.IdPhone == id);
    }
}
