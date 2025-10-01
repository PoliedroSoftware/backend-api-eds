using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Common.Services.Cache;
using Poliedro.Eds.Application.Court.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Product.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.DomainProduct;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Product.DomainProduct.Impl;

public class ProductGetByIdProduct(
    ITenantDbContextFactory dbContextFactory, 
    IRedisService redisService,
    ILogger<ProductGetByIdProduct> logger,
    IHttpContextAccessor httpContextAccessor) : IProductGetByIdProduct
{
    public async Task<Result<ProductEntity, Error>> GetByIdAsync(int id)
    {
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        string cacheKey = $"product:{id}:{tenant}";
        
        try
        {
            // Intentar obtener del cache primero
            var cachedData = await redisService.GetCacheAsync<ProductEntity>(cacheKey);
            if (cachedData is not null)
            {
                logger.LogDebug("📥 Cache HIT para producto {ProductId}", id);
                return cachedData;
            }

            logger.LogDebug("📤 Cache MISS para producto {ProductId} - consultando base de datos", id);

            // Verificar si existe el producto
            if (!await EntityExists(id))
            {
                logger.LogWarning("❌ Producto no encontrado: {ProductId}", id);
                return ProductErrorBuilder.ProductNotFoundException(id);
            }

            // Obtener desde la base de datos
            using var context = dbContextFactory.CreateDbContext();
            var data = await context.Product
                .FirstAsync(c => c.IdProduct == id);

            // Guardar en cache con tags para invalidación distribuida
            var tags = new[] { "product", $"product:{id}" };
            await redisService.SetCacheWithTagsAsync(cacheKey, data, TimeSpan.FromMinutes(1440), tags);

            logger.LogDebug("✅ Producto {ProductId} obtenido de BD y guardado en cache", id);
            return data;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Error al obtener producto {ProductId}: {ErrorMessage}", id, ex.Message);
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
