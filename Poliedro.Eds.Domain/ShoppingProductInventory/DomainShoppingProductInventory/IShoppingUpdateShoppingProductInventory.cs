using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Domain.ShoppingProductInventory.DomainShoppingProductInventory
{
    public interface IShoppingUpdateShoppingProductInventory
    {

        Task<Result<VoidResult, Error>> UpdateAsync(ShoppingProductInventoryEntity shoppingProductInventory);
        
    }
}
