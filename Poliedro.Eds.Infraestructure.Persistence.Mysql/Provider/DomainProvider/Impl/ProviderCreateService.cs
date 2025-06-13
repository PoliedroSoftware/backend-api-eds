using Poliedro.Eds.Application.Provider.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Provider.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Provider.DomainProvider.Impl;

public class ProviderCreateService(DataBaseContext context, IRedisService redisService) : IProviderCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(ProviderEntity ProviderEntity)
    {
        await context.Provider.AddAsync(ProviderEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return ProviderErrorBuilder.ProviderCreationException();
        await redisService.RemoveByPrefixAsync("provider:");
        return VoidResult.Instance;
    }
}