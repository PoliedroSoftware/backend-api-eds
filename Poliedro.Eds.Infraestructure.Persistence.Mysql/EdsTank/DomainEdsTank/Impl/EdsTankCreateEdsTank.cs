using Poliedro.Eds.Application.EdsTank.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.EdsTank.DomainEdsTank;
using Poliedro.Eds.Domain.EdsTank.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EdsTank.DomainEdsTank.Impl;

public class EdsTankCreateEdsTank(ITenantDbContextFactory dbContextFactory) : IEdsTankCreateEdsTank
{
    public async Task<Result<VoidResult, Error>> CreateAsync(EdsTankEntity EdsTankEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.EdsTank.AddAsync(EdsTankEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return EdsTankErrorBuilder.EdsTankCreationException();
        return VoidResult.Instance;
    }
}
