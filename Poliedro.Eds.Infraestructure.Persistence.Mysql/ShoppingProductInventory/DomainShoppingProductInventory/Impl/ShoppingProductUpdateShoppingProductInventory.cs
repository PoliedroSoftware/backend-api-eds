using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Domain.ShoppingProductInventory.DomainShoppingProductInventory;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ShoppingProductInventory.DomainShoppingProductInventory.Impl
{
    public class ShoppingProductUpdateShoppingProductInventory(ITenantDbContextFactory dbContextFactory) : IShoppingUpdateShoppingProductInventory
    {


        public Task<Result<VoidResult, Error>> UpdateAsync(ShoppingProductInventoryEntity shoppingProductInventory)
        {
            throw new NotImplementedException();
        }
    }
}
