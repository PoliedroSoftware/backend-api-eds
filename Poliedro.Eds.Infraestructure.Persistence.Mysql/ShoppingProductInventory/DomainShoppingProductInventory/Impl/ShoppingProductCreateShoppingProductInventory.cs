using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.ShoppingProduct.Errors;
using Poliedro.Eds.Domain.Common.Events;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProductInventory.DomainShoppingProductInventory;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ShoppingProductInventory.DomainShoppingProductInventory.Impl;

public class ShoppingProductCreateShoppingProductInventory(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    ILogger<ShoppingProductCreateShoppingProductInventory> logger,
    IDomainEventDispatcher domainEventDispatcher,
    IHttpContextAccessor httpContextAccessor) : IShoppingCreateShoppingProductInventory
{
    public async Task<Result<VoidResult, Error>> CreateAsync(ShoppingProductInventoryEntity shoppingProductEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        
        try
        {
            logger.LogInformation("=== CREANDO INVENTARIO DE PRODUCTO DE COMPRA ===");
            logger.LogInformation("Shopping Product ID: {ShoppingProductId}, Inventory ID: {InventoryId}",
                shoppingProductEntity.IdShoppingProduct, shoppingProductEntity.IdInventory);

            await context.ShoppingProductInventory.AddAsync(shoppingProductEntity);
            var saveResult = await context.SaveChangesAsync() > 0;
            
            if (!saveResult)
            {
                logger.LogError("❌ ERROR: Falló la creación del inventario de producto de compra");
                return ShoppingProductErrorBuilder.ShoppingProductCreationException();
            }

            logger.LogInformation("✅ Inventario de producto de compra creado exitosamente");

            var result = Result<VoidResult, Error>.Success(VoidResult.Instance);

            // Invalidar caché usando sistema distribuido
            await RedisHelper.InvalidateDistributedCacheAsync(
                result,
                redisService,
                domainEventDispatcher,
                httpContextAccessor,
                "shoppingProductInventory",
                "create",
                shoppingProductEntity.IdShoppingProductInventory);

            // Invalidar caché del inventario general
            await RedisHelper.InvalidateDistributedCacheAsync(
                result,
                redisService,
                domainEventDispatcher,
                httpContextAccessor,
                "inventory",
                "update",
                null);

            // También invalidar el sistema de cache tradicional como respaldo
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, 
                KeyRedisConstants.SHOPPING_PRODUCT_INVENTORY,
                KeyRedisConstants.INVENTORY,
                "inventoryListService:");

            logger.LogInformation("✅ Cache invalidado usando sistema distribuido");
            logger.LogInformation("======================================================");

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ ERROR CRÍTICO: Error al crear inventario de producto de compra - {ErrorMessage}", ex.Message);
            throw;
        }
    }
}
