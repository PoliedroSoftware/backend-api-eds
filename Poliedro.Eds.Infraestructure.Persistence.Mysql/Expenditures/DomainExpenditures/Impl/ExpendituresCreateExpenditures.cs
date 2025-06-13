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

public class ExpendituresCreateExpenditures(DataBaseContext context, IRedisService redisService) : IExpendituresCreateExpenditures
{
    public async Task<Result<VoidResult, Error>> CreateAsync(ExpendituresEntity ExpendituresEntity)
    {
        await context.Expenditures.AddAsync(ExpendituresEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return ExpendituresErrorBuilder.ExpendituresCreationException();
        await redisService.RemoveByPrefixAsync("expenditures:");
        return VoidResult.Instance;
    }
}
