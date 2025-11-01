using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Court.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Events;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.DomainProduct;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Product.DomainProduct.Impl;

public class ProductUpdateProduct(
    ITenantDbContextFactory dbContextFactory, 
    IRedisService redisService,
    ILogger<ProductUpdateProduct> logger,
    IDomainEventDispatcher domainEventDispatcher,
    IHttpContextAccessor httpContextAccessor) : IProductUpdateProduct
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(ProductEntity productEntity)
    {
        if (!await EntityExists(productEntity.IdProduct))
        {
            logger.LogWarning("❌ Producto no encontrado: {ProductId}", productEntity.IdProduct);
            return CourtErrorBuilder.CourtNotFoundException(productEntity.IdProduct);
        }

        using var context = dbContextFactory.CreateDbContext();
        
        try
        {
            logger.LogInformation("=== ACTUALIZANDO PRODUCTO ===");
            logger.LogInformation("Producto ID: {ProductId}, Nombre: {ProductName}", 
                productEntity.IdProduct, productEntity.Name);

            // Obtener el producto actual para comparar cambios
            var currentProduct = await context.Product
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdProduct == productEntity.IdProduct);

            bool stockChanged = false;
            bool priceChanged = false;

            if (currentProduct != null)
            {
                stockChanged = Math.Abs((currentProduct.Stock ?? 0) - (productEntity.Stock ?? 0)) > 0.001m;
                priceChanged = Math.Abs((currentProduct.SellPrice ?? 0) - (productEntity.SellPrice ?? 0)) > 0.01m ||
                              Math.Abs((currentProduct.PurchasePrice ?? 0) - (productEntity.PurchasePrice ?? 0)) > 0.01m;

                if (stockChanged)
                {
                    logger.LogInformation("📦 Stock cambiado: {OldStock} -> {NewStock}",
                        currentProduct.Stock, productEntity.Stock);
                }

                if (priceChanged)
                {
                    logger.LogInformation("💰 Precios cambiados: Venta {OldSell} -> {NewSell}, Compra {OldPurchase} -> {NewPurchase}",
                        currentProduct.SellPrice, productEntity.SellPrice,
                        currentProduct.PurchasePrice, productEntity.PurchasePrice);
                }
            }

            context.Product.Update(productEntity);

            if (await context.SaveChangesAsync() <= 0)
            {
                logger.LogError("❌ ERROR: Falló la actualización del producto");
                return CourtErrorBuilder.CourtUpdateException();
            }

            logger.LogInformation("✅ Producto actualizado exitosamente");

            var result = Result<VoidResult, Error>.Success(VoidResult.Instance);

            // Invalidar caché del producto usando sistema distribuido
            await RedisHelper.InvalidateDistributedCacheAsync(
                result,
                redisService,
                domainEventDispatcher,
                httpContextAccessor,
                "product",
                "update",
                productEntity.IdProduct);

            // Si cambió el stock, también invalidar el inventario
            if (stockChanged)
            {
                await RedisHelper.InvalidateDistributedCacheAsync(
                    result,
                    redisService,
                    domainEventDispatcher,
                    httpContextAccessor,
                    "inventory",
                    "update",
                    null);

                // También invalidar compartimentos ya que el stock puede afectar la información de compartimentos
                await RedisHelper.InvalidateDistributedCacheAsync(
                    result,
                    redisService,
                    domainEventDispatcher,
                    httpContextAccessor,
                    "compartiment",
                    "update",
                    null);
            }

            // También invalidar el sistema de cache tradicional como respaldo
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, 
                KeyRedisConstants.PRODUCT);

            if (stockChanged)
            {
                await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, 
                    KeyRedisConstants.INVENTORY,
                    "inventoryListService:",
                    KeyRedisConstants.COMPARTIMENT);
            }

            logger.LogInformation("✅ Cache invalidado usando sistema distribuido");
            logger.LogInformation("===============================================");

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ ERROR CRÍTICO: Error al actualizar producto - {ErrorMessage}", ex.Message);
            throw;
        }
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Product
            .AsNoTracking()
            .AnyAsync(c => c.IdProduct == id);
    }
}
