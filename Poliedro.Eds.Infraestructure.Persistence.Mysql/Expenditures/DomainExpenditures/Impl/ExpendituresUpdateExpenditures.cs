using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Expenditures.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Expenditures.DomainExpenditures;
using Poliedro.Eds.Domain.Expenditures.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Expenditures.DomainExpenditures.Impl;

public class ExpendituresUpdateExpenditures(DataBaseContext context, IRedisService redisService) : IExpendituresUpdateExpenditures
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(ExpendituresEntity ExpendituresEntity)
    {
        if (!await EntityExists(ExpendituresEntity.IdExpenditures))
            return ExpendituresErrorBuilder.ExpendituresNotFoundException(ExpendituresEntity.IdExpenditures);

        context.Expenditures.Update(ExpendituresEntity);

        if (await context.SaveChangesAsync() <= 0)
            return ExpendituresErrorBuilder.ExpendituresUpdateException();
        await redisService.RemoveByPrefixAsync("expenditures:");
        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Expenditures
            .AsNoTracking()
            .AnyAsync(c => c.IdExpenditures == id);
    }
}
