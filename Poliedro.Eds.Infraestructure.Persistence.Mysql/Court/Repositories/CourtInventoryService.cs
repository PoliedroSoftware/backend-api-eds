using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Domain.Court.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories;

public class CourtInventoryService(IConfiguration config,
    IRedisService redisService,
    ITenantDbContextFactory dbContextFactory) : ICourtUpdateInventoryService
{
    private readonly string _connectionString = config["ConnectionStrings:MysqlConnection"];

    public async Task<Result<VoidResult, Error>> CourtUpdateInventoryAsync(IEnumerable<CourtDispenserSaleEntity> courtDispensers)
    {
        using var context = dbContextFactory.CreateDbContext();

        foreach (var dispenser in courtDispensers)
        {
            // Ahora obtenemos el producto directamente
            var product = await context.Product
                .FirstOrDefaultAsync(p => p.IdProduct == dispenser.IdProduct);

            if (product != null)
            if (product != null)
            {
                Console.WriteLine($"Actualizando producto {product.IdProduct}: Stock antes: {product.Stock}, Vendidos: {dispenser.GallonsDifferenceResult}");

                var nuevoStock = product.Stock - dispenser.GallonsDifferenceResult;
                if (nuevoStock < 0)
                {
                    throw new InvalidOperationException(
                        $"El stock no puede ser negativo para el producto {dispenser.IdProduct}. Stock actual: {product.Stock}, Cantidad vendida: {dispenser.GallonsDifferenceResult}");
                }
                product.Stock = nuevoStock;
                context.Product.Update(product);
            }
        }
        await context.SaveChangesAsync();
        return Result<VoidResult, Error>.Success(VoidResult.Instance);
    }
}
