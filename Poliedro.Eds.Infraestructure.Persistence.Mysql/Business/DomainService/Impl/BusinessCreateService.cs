using Poliedro.Eds.Application.Business.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;

public class BusinessCreateService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IBusinessCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(BusinessEntity BusinessEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.Business.AddAsync(BusinessEntity);

        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return BusinessErrorBuilder.BusinessCreationException();
        await redisService.RemoveByPrefixAsync("business:");
        return VoidResult.Instance;
    }
}