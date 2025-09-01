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
<<<<<<< HEAD
            var product = await context.Product
                .FirstOrDefaultAsync(p => p.IdProduct == dispenser.IdProduct);

            if (product != null)
            {
                Console.WriteLine($"Actualizando producto {product.IdProduct}: Stock antes: {product.Stock}, Vendidos: {dispenser.GallonsDifferenceResult}");

                var nuevoStock = product.Stock - dispenser.GallonsDifferenceResult;
                if (nuevoStock < 0)
                {
                    throw new InvalidOperationException(
                        $"El stock no puede ser negativo para el producto {dispenser.IdProduct}.");
                }
                product.Stock = nuevoStock;
                context.Product.Update(product);
=======
            var productCompartiment = await context.ProductCompartiment
                .FirstOrDefaultAsync(pc => pc.IdProduct == dispenser.IdProduct && pc.IdCompartiment == dispenser.IdCompartiment);

            if (productCompartiment != null)
            {

                Console.WriteLine($"Actualizando product_compartiment {productCompartiment.IdProductCompartiment}: Stock antes: {productCompartiment.Stock}, Vendidos: {dispenser.GallonsDifferenceResult}");

                var nuevoStock = productCompartiment.Stock - dispenser.GallonsDifferenceResult;
                if (nuevoStock < 0)
                {
                    throw new InvalidOperationException(
                        $"El stock no puede ser negativo para el producto {dispenser.IdProduct} en el compartimento {dispenser.IdCompartiment}.");
                }
                productCompartiment.Stock = nuevoStock;
                context.ProductCompartiment.Update(productCompartiment);
>>>>>>> New-service-StrongBox
            }
        }
        await context.SaveChangesAsync();
        return Result<VoidResult, Error>.Success(VoidResult.Instance);
    }
}
