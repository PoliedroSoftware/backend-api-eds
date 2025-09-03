using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Shopping.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
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
                var stockUpdateResult = await UpdateProductStockAsync(context, shoppingEntity.ShoppingProducts);
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
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.COMPARTIMENT);
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.TANK);

            return result;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
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
                product.PurchasePrice = productToUpdate.PurchasePrice;
                product.SellPrice = productToUpdate.SellPrice;
                context.Product.Update(product);
            }
        }

        return Result<VoidResult, Error>.Success(VoidResult.Instance);
    }

    private async Task<Result<VoidResult, Error>> UpdateProductStockAsync(
        DataBaseContext context,
        IEnumerable<ShoppingProductEntity> shoppingProducts)
    {
        var productIds = shoppingProducts.Select(sp => sp.IdProduct).Distinct().ToList();
        var compartimentIds = shoppingProducts.Select(sp => sp.IdCompartment).Distinct().ToList();

        var products = await context.Product
            .Where(p => productIds.Contains(p.IdProduct))
            .ToListAsync();

        var compartiments = await context.Compartiment
            .Where(c => compartimentIds.Contains(c.IdCompartiment))
            .ToListAsync();

        var missingProductIds = productIds.Except(products.Select(p => p.IdProduct)).ToList();
        if (missingProductIds.Any())
        {
            return Error.BadRequest("ProductNotFound", 
                $"No se encontraron los productos con Ids: {string.Join(", ", missingProductIds)}");
        }

        var missingCompartimentIds = compartimentIds.Except(compartiments.Select(c => c.IdCompartiment)).ToList();
        if (missingCompartimentIds.Any())
        {
            return Error.BadRequest("CompartimentNotFound", 
                $"No se encontraron los compartimentos con Ids: {string.Join(", ", missingCompartimentIds)}");
        }

        foreach (var shoppingProduct in shoppingProducts)
        {
            var product = products.First(p => p.IdProduct == shoppingProduct.IdProduct);
            var compartiment = compartiments.First(c => c.IdCompartiment == shoppingProduct.IdCompartment);

            var nuevoStock = product.Stock + shoppingProduct.Quantity;
            if (nuevoStock > compartiment.Operative)
            {
                return Error.BadRequest(
                    "CompartimentCapacityExceeded",
                    $"La suma de stock ({nuevoStock} gls) supera la capacidad operativa ({compartiment.Operative} gls) del compartimento {compartiment.Number} para el producto {product.Name}.");
            }
        }

        foreach (var shoppingProduct in shoppingProducts)
        {
            var product = products.First(p => p.IdProduct == shoppingProduct.IdProduct);
            product.Stock += shoppingProduct.Quantity;
            context.Product.Update(product);
        }

        return Result<VoidResult, Error>.Success(VoidResult.Instance);
    }
}
