using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Dtos
{
    public record ShoppingProductInventoryDto(int IdShoppingProductInventory, int IdShoppingProduct, int IdInventory);
    
}
