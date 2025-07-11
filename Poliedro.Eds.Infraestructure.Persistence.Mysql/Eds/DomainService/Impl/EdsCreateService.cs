using Poliedro.Eds.Application.Eds.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Eds.DomainEds.Impl;

public class EdsCreateService(DataBaseContext context, IRedisService redisService) : IEdsCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(EdsEntity EdsEntity)
    {
        await context.Eds.AddAsync(EdsEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return EdsErrorBuilder.EdsCreationException();
        await redisService.RemoveByPrefixAsync("eds:");
        return VoidResult.Instance;
    }
}