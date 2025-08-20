using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;

namespace Poliedro.Eds.Domain.ShoppingProductInventory.DomainShoppingProductInventory
{
    public interface IShoppingProductInventoryGetById
    {
        Task<Result<ShoppingProductInventoryEntity, Error>> GetByIdAsync(int id);
    }
}
