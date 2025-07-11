using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.EdsTank.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.EdsTank.DomainEdsTank;
using Poliedro.Eds.Domain.EdsTank.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EdsTank.DomainEdsTank.Impl;

public class EdsTankCreateEdsTank(DataBaseContext context, IRedisService redisService) : IEdsTankCreateEdsTank
{
    public async Task<Result<VoidResult, Error>> CreateAsync(EdsTankEntity EdsTankEntity)
    {
        await context.EdsTank.AddAsync(EdsTankEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return EdsTankErrorBuilder.EdsTankCreationException();
        await redisService.RemoveByPrefixAsync("edsTank:");
        return VoidResult.Instance;
    }
}
