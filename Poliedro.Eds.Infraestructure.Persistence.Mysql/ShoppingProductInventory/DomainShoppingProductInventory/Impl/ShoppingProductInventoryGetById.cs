using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.ShoppingProduct.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
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
    public class ShoppingProductInventoryGetById(DataBaseContext context) : IShoppingProductInventoryGetById
    {
        public async Task<Result<ShoppingProductInventoryEntity, Error>> GetByIdAsync(int id)
        {
            if (!await EntityExists(id))
                return ShoppingProductErrorBuilder.ShoppingProductNotFoundException(id);

            return await context.ShoppingProductInventory
                .FirstAsync(c => c.IdShoppingProductInventory == id);
            { }
        }

        private async Task<bool> EntityExists(int id)
        {
            return await context.ShoppingProductInventory
                .AsNoTracking()
                .AnyAsync(c => c.IdShoppingProductInventory == id);
        }
    }
}
