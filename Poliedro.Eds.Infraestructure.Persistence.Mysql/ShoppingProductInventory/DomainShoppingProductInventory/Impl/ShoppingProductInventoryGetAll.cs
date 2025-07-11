using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
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
    public class ShoppingProductInventoryGetAll(ITenantDbContextFactory dbContextFactory) : IShoppingProductInventoryGetAll
    {
        public async Task<IEnumerable<ShoppingProductInventoryEntity>> GetAllAsync(PaginationParams paginationParams)
        {
            using var context = dbContextFactory.CreateDbContext();
            return await context.ShoppingProductInventory
           .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
           .Take(paginationParams.PageSize)
           .ToListAsync();
        }
    }
}
