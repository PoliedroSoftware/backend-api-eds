using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.ProductType.DomainServices;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories;

public class GetProductTypeService : IGetProductTypeName
{
    private readonly ITenantDbContextFactory _dbContextFactory;

    public GetProductTypeService(ITenantDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<string> GetProductTypeNameAsync(int idProduct)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var Product = await context.Product
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdProduct == idProduct);

        if (Product == null) return "Desconocido";

        var product = await context.Product
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdProduct == Product.IdProduct);

        var productType = await context.ProductType
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdProductType == product.IdProductType);

        return $"{product?.Name ?? "Desconocido"} - {productType?.Description ?? "Desconocido"}";
    }
}

