using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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

public class ProductCreateProduct(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    ILogger<ProductCreateProduct> logger,
    IDomainEventDispatcher domainEventDispatcher,
    IHttpContextAccessor httpContextAccessor) : IProductCreateProduct
{
    public async Task<Result<VoidResult, Error>> CreateAsync(ProductEntity productEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        
        try
        {
            logger.LogInformation("=== CREANDO PRODUCTO ===");
            logger.LogInformation("Nombre: {ProductName}, Tipo: {ProductType}, Stock: {Stock}, Precio Venta: {SellPrice}",
                productEntity.Name, productEntity.IdProductType, productEntity.Stock, productEntity.SellPrice);

            await context.Product.AddAsync(productEntity);
            var result = await context.SaveChangesAsync() > 0;
            
            if (!result)
            {
                logger.LogError("❌ ERROR: Falló la creación del producto");
                return CourtErrorBuilder.CourtCreationException();
            }

            logger.LogInformation("✅ Producto creado exitosamente con ID: {ProductId}", productEntity.IdProduct);

            var successResult = Result<VoidResult, Error>.Success(VoidResult.Instance);

            // Invalidar caché del producto usando sistema distribuido
            await RedisHelper.InvalidateDistributedCacheAsync(
                successResult,
                redisService,
                domainEventDispatcher,
                httpContextAccessor,
                "product",
                "create",
                productEntity.IdProduct);

            // También invalidar el inventario ya que se creó un nuevo producto
            await RedisHelper.InvalidateDistributedCacheAsync(
                successResult,
                redisService,
                domainEventDispatcher,
                httpContextAccessor,
                "inventory",
                "update",
                null);

            // También invalidar el sistema de cache tradicional como respaldo
            await RedisHelper.RemoveCacheIfSuccessAsync(successResult, redisService, 
                KeyRedisConstants.PRODUCT,
                KeyRedisConstants.INVENTORY,
                "inventoryListService:");

            logger.LogInformation("✅ Cache invalidado usando sistema distribuido");
            logger.LogInformation("==========================================");

            return successResult;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ ERROR CRÍTICO: Error al crear producto - {ErrorMessage}", ex.Message);
            throw;
        }
    }
}
