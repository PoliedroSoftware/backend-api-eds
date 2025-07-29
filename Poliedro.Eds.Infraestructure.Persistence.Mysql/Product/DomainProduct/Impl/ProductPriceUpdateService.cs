using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Product.Services;
using Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Product.Services;

public class ProductPriceUpdateService : IProductPriceUpdateService
{
    private readonly DataBaseContext _context;

    public ProductPriceUpdateService(DataBaseContext context)
    {
        _context = context;
    }

    public async Task<Result<VoidResult, Error>> UpdatePricesAsync(IEnumerable<ProductEntity> products)
    {
        foreach (var item in products)
        {
            var product = await _context.Product.FirstOrDefaultAsync(p => p.IdProduct == item.IdProduct);
            if (product == null)
                return Result<VoidResult, Error>.Failure(Error.BadRequest("ProductNotFound", $"No se encontró el producto con Id {item.IdProduct}").Error!);

            product.Price = item.Price;
        }

        await _context.SaveChangesAsync();
        return Result<VoidResult, Error>.Success(VoidResult.Instance);
    }
}
