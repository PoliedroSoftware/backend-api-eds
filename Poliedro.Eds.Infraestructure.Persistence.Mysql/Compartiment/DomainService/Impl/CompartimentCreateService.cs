using Poliedro.Eds.Application.Compartiment.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Compartiment.DomainCompartiment.Impl;

public class CompartimentCreateService(ITenantDbContextFactory dbContextFactory) : ICompartimentCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(CompartimentEntity compartimentEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.Compartiment.AddAsync(compartimentEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return CompartimentErrorBuilder.CompartimentCreationException();
        return VoidResult.Instance;
    }
}