using Poliedro.Eds.Application.ShoppingProduct.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Domain.ShoppingProductInventory.DomainShoppingProductInventory;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ShoppingProductInventory.DomainShoppingProductInventory.Impl
{
    public class ShoppingProductCreateShoppingProductInventory(DataBaseContext context) : IShoppingCreateShoppingProductInventory
    {
        public async Task<Result<VoidResult, Error>> CreateAsync(ShoppingProductInventoryEntity shoppingProductEntity)
        {
            //throw new NotImplementedException();

            await context.ShoppingProductInventory.AddAsync(shoppingProductEntity);
            var result = await context.SaveChangesAsync() > 0;
            if (!result)
                return ShoppingProductErrorBuilder.ShoppingProductCreationException();

            return VoidResult.Instance;
        }
    }
}
