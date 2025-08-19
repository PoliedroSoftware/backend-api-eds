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
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Domain.Shopping.DomainShopping;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Shopping.DomainShopping.Impl;

public class ProductPriceUpdateService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IProductPriceUpdateService
{
    public async Task<Result<VoidResult, Error>> UpdatePricesAsync(IEnumerable<ProductEntity> products)
    {
        var productIds = products.Select(p => p.IdProduct).ToList();
        var nonExistingIds = await GetNonExistingProductIdsAsync(productIds);

        if (nonExistingIds.Any())
            return Error.BadRequest("ProductNotFound", $"No se encontraron los productos con Ids: {string.Join(", ", nonExistingIds)}").Error!;

        using var context = dbContextFactory.CreateDbContext();

        bool anyChange = false;
        foreach (var item in products)
        {
            var product = await context.Product.FirstOrDefaultAsync(p => p.IdProduct == item.IdProduct);
            if (product != null)
            {
                product.Price = item.Price;
                anyChange = true;
            }
        }

        if (!anyChange)
            return Result<VoidResult, Error>.Success(VoidResult.Instance);

        try
        {
            var saveResult = await context.SaveChangesAsync();
            //if (saveResult <= 0)
            //    return ShoppingErrorBuilder.ShoppingCreationException();

            var result = Result<VoidResult, Error>.Success(VoidResult.Instance);
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.PRODUCT);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al guardar cambios: " + ex.Message);
            if (ex.InnerException != null)
                Console.WriteLine("Inner exception: " + ex.InnerException.Message);

            return Error.Internal("ProductPriceUpdateError", $"Error al guardar cambios: {ex.Message}");
        }
    }

    private async Task<List<int>> GetNonExistingProductIdsAsync(IEnumerable<int> ids)
    {
        using var context = dbContextFactory.CreateDbContext();
        var existingIds = await context.Product
            .AsNoTracking()
            .Where(p => ids.Contains(p.IdProduct))
            .Select(p => p.IdProduct)
            .ToListAsync();

        return ids.Except(existingIds).ToList();
    }
}
