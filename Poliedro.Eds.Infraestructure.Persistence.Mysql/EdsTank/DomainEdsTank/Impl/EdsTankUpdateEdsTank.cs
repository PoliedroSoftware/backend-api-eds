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

public class EdsTankUpdateEdsTank(DataBaseContext context, IRedisService redisService) : IEdsTankUpdateEdsTank
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(EdsTankEntity EdsTankEntity)
    {
        if (!await EntityExists(EdsTankEntity.IdEdsTank))
            return EdsTankErrorBuilder.EdsTankNotFoundException(EdsTankEntity.IdEdsTank);

        context.EdsTank.Update(EdsTankEntity);

        if (await context.SaveChangesAsync() <= 0)
            return EdsTankErrorBuilder.EdsTankUpdateException();
        await redisService.RemoveByPrefixAsync("edsTank:");
        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.EdsTank
            .AsNoTracking()
            .AnyAsync(c => c.IdEdsTank == id);
    }
}
