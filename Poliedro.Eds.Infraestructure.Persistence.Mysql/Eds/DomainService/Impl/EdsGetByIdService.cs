using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Eds.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Eds.DomainEds.Impl;

public class EdsGetByIdService(ITenantDbContextFactory dbContextFactory) : IEdsGetByIdService
{
    public async Task<Result<EdsEntity, Error>> GetByIdAsync(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        
        var data = await context.Eds
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdEds == id);

        if (data is null)
            return EdsErrorBuilder.EdsNotFoundException(id);

        return data;
    }
}
