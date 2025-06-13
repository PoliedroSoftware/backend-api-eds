using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories;

public class CourtInventoryService : ICourtUpdateInventoryService
{
    private readonly string _connectionString;
    private readonly IRedisService _redisService;
    private readonly DataBaseContext _context;

    public CourtInventoryService(IConfiguration config, IRedisService redisService, DataBaseContext context)
    {
        _connectionString = config["ConnectionStrings:MysqlConnection"];
        _redisService = redisService;
        _context = context;
    }

    public async Task CourtUpdateInventoryAsync(IEnumerable<ICourtDispenserSaleEntity> courtDispensers)
    {
        foreach (var dispenser in courtDispensers)
        {
            var compartment = await _context.Compartiment
                .FirstOrDefaultAsync(c => c.IdCompartment == dispenser.IdCompartiment);

            if (compartment != null)
            {
                Console.WriteLine($"Actualizando compartimiento {compartment.IdCompartment}: Stock antes: {compartment.Stock}, Vendidos: {dispenser.GallonsDifferenceResult}");

                compartment.Stock -= dispenser.GallonsDifferenceResult;
                _context.Compartiment.Update(compartment);
            }
        }

        await _context.SaveChangesAsync();
    }

    public Task CourtUpdateInventoryAsync(IEnumerable<CourtDispenserEntity> courtDispensers)
    {
        throw new NotImplementedException();
    }
}