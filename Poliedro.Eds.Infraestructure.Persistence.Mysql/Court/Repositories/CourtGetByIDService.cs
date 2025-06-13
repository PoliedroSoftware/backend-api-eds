using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Court.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories;

internal class CourtGetByIDService(DataBaseContext context) : ICourtGetByIdDomainService
{
    public async Task<Result<CourtEntity, Error>> GetByIdAsync(int id)
    {
        if (!await EntityExists(id))
            return CourtErrorBuilder.CourtNotFoundException(id);

        var courtValue = await context.Court
                        .Include(c => c.CourtDispensers)
                        .Include(c => c.CourtDocuments)
                        .Include(c => c.CourtExpenditures)
                        .Include(c => c.CourtTypeOfCollections)
                            .ThenInclude(ct => ct.TypeOfCollection)
                        .FirstOrDefaultAsync(c => c.IdCourt == id);
        return courtValue;
    }

    private async Task<bool> EntityExists(int id)
    {
        return await context.Court
            .AsNoTracking()
            .AnyAsync(c => c.IdCourt == id);
    }
}
