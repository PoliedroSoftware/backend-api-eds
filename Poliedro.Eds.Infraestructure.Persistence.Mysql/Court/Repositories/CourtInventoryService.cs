using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories;

public class CourtInventoryService(IConfiguration config,
    IRedisService redisService,
    ITenantDbContextFactory dbContextFactory) : ICourtUpdateInventoryService
{
    public async Task CourtUpdateInventoryAsync(IEnumerable<ICourtDispenserSaleEntity> courtDispensers)
    {

        using var context = dbContextFactory.CreateDbContext();

        foreach (var dispenser in courtDispensers)
        {
            var compartment = await context.Compartiment
                .FirstOrDefaultAsync(c => c.IdCompartment == dispenser.IdCompartiment);

            if (compartment is not null)
            {
                Console.WriteLine($"Actualizando compartimiento {compartment.IdCompartment}: Stock antes: {compartment.Stock}, Vendidos: {dispenser.GallonsDifferenceResult}");

                compartment.Stock -= dispenser.GallonsDifferenceResult;
                context.Compartiment.Update(compartment);
            }
        }

        await context.SaveChangesAsync();
    }

    public Task CourtUpdateInventoryAsync(IEnumerable<CourtDispenserEntity> courtDispensers)
    {
        throw new NotImplementedException();
    }
}
