using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Domain.ShoppingProductInventory.DomainShoppingProductInventory
{
    public interface IShoppingProductInventoryGetAll
    {

        Task<IEnumerable<ShoppingProductInventoryEntity>> GetAllAsync(PaginationParams paginationParams);

    }
}
