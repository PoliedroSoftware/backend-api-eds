using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Shopping.Errors;
using Poliedro.Eds.Domain.Common.Events;
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
    IRedisService redisService,
    ILogger<ShoppingTransactionalService> logger,
    IDomainEventDispatcher domainEventDispatcher,
    IHttpContextAccessor httpContextAccessor) : IShoppingTransactionalService
{
    public async Task<Result<VoidResult, Error>> ExecuteShoppingTransactionAsync(
        ShoppingEntity shoppingEntity,
        IEnumerable<ProductEntity> productsToUpdatePrice)
    {
        using var context = dbContextFactory.CreateDbContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            logger.LogInformation("=== INICIANDO TRANSACCIÓN DE COMPRA ===");
            logger.LogInformation("Compra ID: {ShoppingId}, Productos: {ProductCount}, Precios a actualizar: {PriceUpdateCount}",
                shoppingEntity.IdShopping, 
                shoppingEntity.ShoppingProducts?.Count() ?? 0,
                productsToUpdatePrice.Count());

            // 1. Actualizar precios de productos si es necesario
            if (productsToUpdatePrice.Any())
            {
                var priceUpdateResult = await UpdateProductPricesAsync(context, productsToUpdatePrice);
                if (!priceUpdateResult.IsSuccess)
                {
                    await transaction.RollbackAsync();
                    logger.LogError("❌ ERROR: Falló la actualización de precios - {ErrorDescription}", 
                        priceUpdateResult.Error?.Description);
                    return priceUpdateResult.Error!;
                }
                logger.LogInformation("✅ Precios actualizados exitosamente");
            }

            // 2. Actualizar stock de productos (incrementar)
            if (shoppingEntity.ShoppingProducts?.Any() == true)
            {
                var stockUpdateResult = await UpdateProductStockAsync(context, shoppingEntity.ShoppingProducts);
                if (!stockUpdateResult.IsSuccess)
                {
                    await transaction.RollbackAsync();
                    logger.LogError("❌ ERROR: Falló la actualización de stock - {ErrorDescription}", 
                        stockUpdateResult.Error?.Description);
                    return stockUpdateResult.Error!;
                }
                logger.LogInformation("✅ Stock actualizado exitosamente");
            }

            // 3. Crear el registro de compra
            await context.Shopping.AddAsync(shoppingEntity);
            var shoppingSaveResult = await context.SaveChangesAsync() > 0;
            if (!shoppingSaveResult)
            {
                await transaction.RollbackAsync();
                logger.LogError("❌ ERROR: Falló la creación de la compra");
                return ShoppingErrorBuilder.ShoppingCreationException();
            }
            logger.LogInformation("✅ Compra creada exitosamente");

            // 4. Confirmar la transacción
            await transaction.CommitAsync();
            logger.LogInformation("✅ TRANSACCIÓN DE COMPRA CONFIRMADA EXITOSAMENTE");

            var result = Result<VoidResult, Error>.Success(VoidResult.Instance);
            
            // 5. Invalidar caché de compra usando sistema distribuido
            await RedisHelper.InvalidateDistributedCacheAsync(
                result,
                redisService,
                domainEventDispatcher,
                httpContextAccessor,
                "shopping",
                "create",
                shoppingEntity.IdShopping);

            // 6. Invalidar caché del inventario y productos afectados
            await InvalidateInventoryAndProductCacheAsync(result, shoppingEntity.ShoppingProducts, productsToUpdatePrice);

            logger.LogInformation("✅ Cache invalidado usando sistema distribuido");
            logger.LogInformation("================================================");

            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "❌ ERROR CRÍTICO: Rollback ejecutado - {ErrorMessage}", ex.Message);
            logger.LogInformation("================================================");
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

        var priceUpdates = new List<(int ProductId, double? OldPurchasePrice, double? NewPurchasePrice, double? OldSellPrice, double? NewSellPrice, string ProductName)>();

        foreach (var productToUpdate in productsToUpdate)
        {
            var product = await context.Product
                .FirstOrDefaultAsync(p => p.IdProduct == productToUpdate.IdProduct);
            
            if (product != null)
            {
                var oldPurchasePrice = product.PurchasePrice;
                var oldSellPrice = product.SellPrice;

                product.PurchasePrice = productToUpdate.PurchasePrice;
                product.SellPrice = productToUpdate.SellPrice;
                context.Product.Update(product);

                priceUpdates.Add((product.IdProduct, oldPurchasePrice, productToUpdate.PurchasePrice, 
                    oldSellPrice, productToUpdate.SellPrice, product.Name));
            }
        }

        // Log de auditoría de precios actualizados
        if (priceUpdates.Any())
        {
            logger.LogInformation("=== PRECIOS ACTUALIZADOS DESDE COMPRA ===");
            foreach (var (productId, oldPurchase, newPurchase, oldSell, newSell, productName) in priceUpdates)
            {
                logger.LogInformation("💰 Producto {ProductId} ({ProductName}): Compra ${OldPurchase:F2} -> ${NewPurchase:F2}, Venta ${OldSell:F2} -> ${NewSell:F2}", 
                    productId, productName, oldPurchase ?? 0, newPurchase ?? 0, oldSell ?? 0, newSell ?? 0);
            }
            logger.LogInformation("Total productos con precios actualizados: {UpdateCount}", priceUpdates.Count);
            logger.LogInformation("===========================================");
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

        // Validar capacidad antes de actualizar
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

        // Actualizar stock
        foreach (var shoppingProduct in shoppingProducts)
        {
            var product = products.First(p => p.IdProduct == shoppingProduct.IdProduct);
            var oldStock = product.Stock;
            product.Stock += shoppingProduct.Quantity;
            context.Product.Update(product);

            logger.LogInformation("📦 Stock actualizado para producto {ProductId}: {OldStock} -> {NewStock} (Comprados: {PurchasedQuantity} gal)",
                product.IdProduct,
                oldStock,
                product.Stock,
                shoppingProduct.Quantity);
        }

        return Result<VoidResult, Error>.Success(VoidResult.Instance);
    }

    /// <summary>
    /// Invalida la caché del inventario y productos afectados por la compra
    /// </summary>
    private async Task InvalidateInventoryAndProductCacheAsync(
        Result<VoidResult, Error> result,
        IEnumerable<ShoppingProductEntity>? shoppingProducts,
        IEnumerable<ProductEntity> productsToUpdatePrice)
    {
        if (!result.IsSuccess) return;

        try
        {
            // Invalidar cache del inventario usando sistema distribuido
            await RedisHelper.InvalidateDistributedCacheAsync(
                result,
                redisService,
                domainEventDispatcher,
                httpContextAccessor,
                "inventory",
                "update",
                null);

            // Invalidar cache de productos usando sistema distribuido
            await RedisHelper.InvalidateDistributedCacheAsync(
                result,
                redisService,
                domainEventDispatcher,
                httpContextAccessor,
                "product",
                "update",
                null);

            // Invalidar cache específico de cada producto afectado por cambio de stock
            if (shoppingProducts?.Any() == true)
            {
                foreach (var shoppingProduct in shoppingProducts)
                {
                    await RedisHelper.InvalidateDistributedCacheAsync(
                        result,
                        redisService,
                        domainEventDispatcher,
                        httpContextAccessor,
                        "product",
                        "update",
                        shoppingProduct.IdProduct);
                }
            }

            // Invalidar cache específico de cada producto afectado por cambio de precio
            foreach (var product in productsToUpdatePrice)
            {
                await RedisHelper.InvalidateDistributedCacheAsync(
                    result,
                    redisService,
                    domainEventDispatcher,
                    httpContextAccessor,
                    "product",
                    "update",
                    product.IdProduct);
            }

            // Invalidar cache de compartimentos ya que el stock puede afectar la información de compartimentos
            await RedisHelper.InvalidateDistributedCacheAsync(
                result,
                redisService,
                domainEventDispatcher,
                httpContextAccessor,
                "compartiment",
                "update",
                null);

            // También invalidar las constantes de cache tradicionales como respaldo
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, 
                KeyRedisConstants.INVENTORY,
                KeyRedisConstants.SHOPPING,
                KeyRedisConstants.PRODUCT, 
                "inventoryListService:",
                KeyRedisConstants.COMPARTIMENT,
                KeyRedisConstants.TANK);

            logger.LogInformation("🗑️ Cache de inventario, productos y compras invalidado exitosamente");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Error al invalidar cache de inventario y productos: {ErrorMessage}", ex.Message);
            // No lanzamos excepción porque la transacción ya fue confirmada exitosamente
        }
    }
}
