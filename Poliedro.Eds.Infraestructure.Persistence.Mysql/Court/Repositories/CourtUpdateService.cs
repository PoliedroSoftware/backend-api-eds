using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Court.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories;

internal class CourtUpdateService(DataBaseContext context) : ICourtUpdateService
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(CourtEntity courtEntity)
    {
        if (!await EntityExists(courtEntity.IdCourt))
            return CourtErrorBuilder.CourtNotFoundException(courtEntity.IdCourt);

        context.Court.Update(courtEntity);

        if (await context.SaveChangesAsync() <= 0)
            return CourtErrorBuilder.CourtUpdateException();

        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Court
            .AsNoTracking()
            .AnyAsync(c => c.IdCourt == id);
    }
}
