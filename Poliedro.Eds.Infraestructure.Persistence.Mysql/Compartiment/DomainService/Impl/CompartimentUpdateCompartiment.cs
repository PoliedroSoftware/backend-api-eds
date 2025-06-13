using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Compartiment.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Compartiment.DomainCompartiment.Impl;

public class CompartimentUpdateCompartiment(DataBaseContext context, IRedisService redisService) : ICompartimentUpdateCompartiment
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(CompartimentEntity compartimentEntity)
    {
        if (!await EntityExists(compartimentEntity.IdCompartment))
            return CompartimentErrorBuilder.CompartimentNotFoundException(compartimentEntity.IdCompartment);

        context.Compartiment.Update(compartimentEntity);

        if (await context.SaveChangesAsync() <= 0)
            return CompartimentErrorBuilder.CompartimentUpdateException();
        await redisService.RemoveByPrefixAsync("compartiment:");

        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Compartiment
            .AsNoTracking()
            .AnyAsync(c => c.IdCompartment == id);
    }
}