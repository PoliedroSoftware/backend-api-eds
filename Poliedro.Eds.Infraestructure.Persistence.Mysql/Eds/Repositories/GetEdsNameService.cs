using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Eds.Repositories;

public class GetEdsNameService(ITenantDbContextFactory dbContextFactory) : IGetEdsName
{
    public async Task<string> GetEdsNameAsync(int idEds)
    {
        await using var context = dbContextFactory.CreateDbContext();
        var eds = await context.Eds
            .AsNoTracking()
            .Where(x => x.IdEds == idEds)
            .Select(x => x.Name)
            .FirstOrDefaultAsync();
            
        return eds ?? "EDS Desconocida";
    }
}
