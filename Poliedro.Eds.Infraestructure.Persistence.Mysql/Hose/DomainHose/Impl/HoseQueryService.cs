
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Hose.DomainHose.Impl
{
    public class HoseQueryService(ITenantDbContextFactory dbContextFactory, ILogger<HoseQueryService> logger) : IHoseQueryService
    {
        public async Task<int?> GetHoseLimitAsync(int dispenserId)
        {
        logger.LogInformation("Processing Hose");
            using var context = dbContextFactory.CreateDbContext();

            var dispenser = await context.Dispensers
                .Where(d => d.Id == dispenserId)
                .Select(d => d.HoseNumber)
                .FirstOrDefaultAsync();

            return dispenser;

        }

        public async Task<int> GetCurrentHoseCountAsync(int dispenserId)
        {
            using var context = dbContextFactory.CreateDbContext();

            int hoseCount = await context.Hose.CountAsync(h => h.IdDispensers == dispenserId);

            return hoseCount;
        }
    }

}
