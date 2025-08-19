using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.ProductCompartiment.Entities;
using Poliedro.Eds.Domain.Compartiment.Entities;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Common.Constants;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Shopping.DomainShopping.Impl;

public class ProductCompartimentStockUpdateService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IProductCompartimentStockUpdate
{
    public async Task<Result<VoidResult, Error>> UpdateStockAsync(IEnumerable<ShoppingProductEntity> shoppingProducts)
    {
        using var context = dbContextFactory.CreateDbContext();

        var productCompartimentKeys = shoppingProducts
            .Select(sp => new { sp.IdProduct, sp.IdCompartment })
            .ToList();

        var productIds = productCompartimentKeys.Select(k => k.IdProduct).Distinct().ToList();
        var compartimentIds = productCompartimentKeys.Select(k => k.IdCompartment).Distinct().ToList();

        var productCompartiments = await context.ProductCompartiment
            .Where(pc => productIds.Contains(pc.IdProduct) && compartimentIds.Contains(pc.IdCompartiment))
            .ToListAsync();

        var compartiments = await context.Compartiment
            .Where(c => compartimentIds.Contains(c.IdCompartment))
            .ToListAsync();

        var productNames = await context.Product
            .Where(p => productIds.Contains(p.IdProduct))
            .ToDictionaryAsync(p => p.IdProduct, p => p.Name);

        foreach (var shoppingProduct in shoppingProducts)
        {
            var productCompartiment = productCompartiments
                .FirstOrDefault(pc => pc.IdProduct == shoppingProduct.IdProduct && pc.IdCompartiment == shoppingProduct.IdCompartment);

            var productName = productNames.TryGetValue(shoppingProduct.IdProduct, out var name) ? name : $"ID {shoppingProduct.IdProduct}";

            var compartiment = compartiments
                .FirstOrDefault(c => c.IdCompartment == shoppingProduct.IdCompartment);

            if (productCompartiment == null)
            {
                return Error.BadRequest(
                    "ProductCompartimentNotFound",
                    $"No se encontró el registro de producto-compartimento para {productName} en el Compartimento {compartiment.Number}.");
            }
            
            if (compartiment == null)
            {
                return Error.BadRequest(
                    "CompartimentNotFound",
                    $"No se encontró el compartimento con Id {shoppingProduct.IdCompartment}.");
            }

            var nuevoStock = productCompartiment.Stock + shoppingProduct.Quantity;
            if (nuevoStock > compartiment.Operative)
            {
                return Error.BadRequest(
                    "CompartimentCapacityExceeded",
                    $"La suma de stock ({nuevoStock} gls) supera la capacidad operativa ({compartiment.Operative} gls) del compartimento {compartiment.Number}.");
            }
        }

        foreach (var shoppingProduct in shoppingProducts)
        {
            var productCompartiment = productCompartiments
                .First(pc => pc.IdProduct == shoppingProduct.IdProduct && pc.IdCompartiment == shoppingProduct.IdCompartment);

            productCompartiment.Stock += shoppingProduct.Quantity;
            context.ProductCompartiment.Update(productCompartiment);
        }

        if (await context.SaveChangesAsync() <= 0)
            return Error.Internal("ProductCompartimentStockUpdateError", "No se pudo actualizar el stock de los compartimentos.");

        var result = Result<VoidResult, Error>.Success(VoidResult.Instance);
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.PRODUCT_COMPARTMENT);
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.COMPARTIMENT);
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.TANK);
        return result;
    }
}
