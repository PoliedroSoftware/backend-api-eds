//using Poliedro.Eds.Domain.Product.Entities;
//using Poliedro.Eds.Domain.ShoppingProduct.Entities;
//using Poliedro.Eds.Domain.Common.Results;
//using Poliedro.Eds.Domain.Common.Results.Errors;


//namespace Poliedro.Eds.Application.Shopping.Commands.CreateShopping;

//public static class ShoppingValidationService
//{
//    public static async Task<Result<VoidResult, Error>> ValidateProductExistenceAsync(
//        ITenantDbContextFactory dbContextFactory,
//        IEnumerable<ProductEntity> products)
//    {
//        var productIds = products.Select(p => p.IdProduct).ToList();
//        using var context = dbContextFactory.CreateDbContext();

//        var existingIds = await context.Product
//            .AsNoTracking()
//            .Where(p => productIds.Contains(p.IdProduct))
//            .Select(p => p.IdProduct)
//            .ToListAsync();

//        var nonExistingIds = productIds.Except(existingIds).ToList();
//        if (nonExistingIds.Any())
//        {
//            return Error.BadRequest("ProductNotFound", $"No se encontraron los productos con Ids: {string.Join(", ", nonExistingIds)}");
//        }

//        return Result<VoidResult, Error>.Success(VoidResult.Instance);
//    }

//    public static async Task<Result<VoidResult, Error>> ValidateCompartimentStockAsync(
//        ITenantDbContextFactory dbContextFactory,
//        IEnumerable<ShoppingProductEntity> shoppingProducts)
//    {
//        var productIds = shoppingProducts.Select(sp => sp.IdProduct).Distinct().ToList();
//        var compartimentIds = shoppingProducts.Select(sp => sp.IdCompartment).Distinct().ToList();
//        using var context = dbContextFactory.CreateDbContext();

//        var productCompartiments = await context.ProductCompartiment
//            .Where(pc => productIds.Contains(pc.IdProduct) && compartimentIds.Contains(pc.IdCompartiment))
//            .ToListAsync();

//        var compartiments = await context.Compartiment
//            .Where(c => compartimentIds.Contains(c.IdCompartment))
//            .ToListAsync();

//        var productNames = await context.Product
//            .Where(p => productIds.Contains(p.IdProduct))
//            .ToDictionaryAsync(p => p.IdProduct, p => p.Name);

//        foreach (var shoppingProduct in shoppingProducts)
//        {
//            var productCompartiment = productCompartiments
//                .FirstOrDefault(pc => pc.IdProduct == shoppingProduct.IdProduct && pc.IdCompartiment == shoppingProduct.IdCompartment);

//            var productName = productNames.TryGetValue(shoppingProduct.IdProduct, out var name) ? name : $"ID {shoppingProduct.IdProduct}";

//            var compartiment = compartiments
//                .FirstOrDefault(c => c.IdCompartment == shoppingProduct.IdCompartment);

//            if (productCompartiment == null)
//            {
//                return Error.BadRequest(
//                    "ProductCompartimentNotFound",
//                    $"No se encontró el registro de producto-compartimento para {productName} en el Compartimento {(compartiment != null ? compartiment.Number : shoppingProduct.IdCompartment.ToString())}.");
//            }

//            if (compartiment == null)
//            {
//                return Error.BadRequest(
//                    "CompartimentNotFound",
//                    $"No se encontró el compartimento con Id {shoppingProduct.IdCompartment}.");
//            }

//            var nuevoStock = productCompartiment.Stock + shoppingProduct.Quantity;
//            if (nuevoStock > compartiment.Operative)
//            {
//                return Error.BadRequest(
//                    "CompartimentCapacityExceeded",
//                    $"La suma de stock ({nuevoStock} gls) supera la capacidad operativa ({compartiment.Operative} gls) del compartimento {compartiment.Number}.");
//            }
//        }

//        return Result<VoidResult, Error>.Success(VoidResult.Instance);
//    }
//}
