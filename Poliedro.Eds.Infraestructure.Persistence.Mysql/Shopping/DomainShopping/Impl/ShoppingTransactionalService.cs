using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Shopping.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductCompartiment.Entities;
using Poliedro.Eds.Domain.Compartiment.Entities;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Domain.Shopping.DomainShopping;
using Poliedro.Eds.Domain.Shopping.Entities;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Shopping.DomainShopping.Impl;

public class ShoppingTransactionalService(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService) : IShoppingTransactionalService
{
    public async Task<Result<VoidResult, Error>> ExecuteShoppingTransactionAsync(
        ShoppingEntity shoppingEntity,
        IEnumerable<ProductEntity> productsToUpdatePrice)
    {
        using var context = dbContextFactory.CreateDbContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            if (productsToUpdatePrice.Any())
            {
                var priceUpdateResult = await UpdateProductPricesAsync(context, productsToUpdatePrice);
                if (!priceUpdateResult.IsSuccess)
                {
                    await transaction.RollbackAsync();
                    return priceUpdateResult.Error!;
                }
            }

            if (shoppingEntity.ShoppingProducts?.Any() == true)
            {
                var stockUpdateResult = await UpdateCompartimentStockAsync(context, shoppingEntity.ShoppingProducts);
                if (!stockUpdateResult.IsSuccess)
                {
                    await transaction.RollbackAsync();
                    return stockUpdateResult.Error!;
                }
            }

            await context.Shopping.AddAsync(shoppingEntity);
            var shoppingSaveResult = await context.SaveChangesAsync() > 0;
            if (!shoppingSaveResult)
            {
                await transaction.RollbackAsync();
                return ShoppingErrorBuilder.ShoppingCreationException();
            }

            await transaction.CommitAsync();

            var result = Result<VoidResult, Error>.Success(VoidResult.Instance);
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.SHOPPING);
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.PRODUCT);
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.PRODUCT_COMPARTMENT);
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.COMPARTIMENT);
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.TANK);

            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Error.Internal("ShoppingTransactionError", $"Error en la transacción de shopping: {ex.Message}");
        }
    }

    private async Task<Result<VoidResult, Error>> UpdateProductPricesAsync(
        DataBaseContext context,
        IEnumerable<ProductEntity> productsToUpdate)
    {
        var productIds = productsToUpdate.Select(p => p.IdProduct).ToList();
        
        var existingIds = await context.Product
            .Where(p => productIds.Contains(p.IdProduct))
            .Select(p => p.IdProduct)
            .ToListAsync();

        var nonExistingIds = productIds.Except(existingIds).ToList();
        if (nonExistingIds.Any())
        {
            return Error.BadRequest("ProductNotFound", 
                $"No se encontraron los productos con Ids: {string.Join(", ", nonExistingIds)}");
        }

        foreach (var productToUpdate in productsToUpdate)
        {
            var product = await context.Product
                .FirstOrDefaultAsync(p => p.IdProduct == productToUpdate.IdProduct);
            
            if (product != null)
            {
                product.Price = productToUpdate.Price;
                context.Product.Update(product);
            }
        }

        return Result<VoidResult, Error>.Success(VoidResult.Instance);
    }

    private async Task<Result<VoidResult, Error>> UpdateCompartimentStockAsync(
        DataBaseContext context,
        IEnumerable<ShoppingProductEntity> shoppingProducts)
    {
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
                var compartimentNumber = compartiment?.Number.ToString() ?? shoppingProduct.IdCompartment.ToString();
                return Error.BadRequest(
                    "ProductCompartimentNotFound",
                    $"No se encontró el registro de producto-compartimento para {productName} en el Compartimento {compartimentNumber}.");
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

        return Result<VoidResult, Error>.Success(VoidResult.Instance);
    }
}
