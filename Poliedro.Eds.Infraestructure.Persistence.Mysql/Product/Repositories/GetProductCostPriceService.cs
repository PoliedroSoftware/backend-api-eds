using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Product.DomainServices;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Product.Repositories;

public class GetProductCostPriceService(ITenantDbContextFactory dbContextFactory) : IGetProductCostPrice
{
    public async Task<double?> GetProductCostPriceAsync(int idProduct)
    {
        using var context = dbContextFactory.CreateDbContext();

        var latestShoppingProduct = await context.ShoppingProduct
            .Where(sp => sp.IdProduct == idProduct)
            .OrderByDescending(sp => sp.CreatedAt)
            .FirstOrDefaultAsync();

        return latestShoppingProduct?.Price;
    }
}
