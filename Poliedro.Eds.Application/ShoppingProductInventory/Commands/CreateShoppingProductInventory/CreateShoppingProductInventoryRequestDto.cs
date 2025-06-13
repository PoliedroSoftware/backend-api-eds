using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Commands.CreateShoppingProductInventory
{
    public record CreateShoppingProductInventoryRequestDto(int IdShoppingProductInventory,
    int IdShoppingProduct,int IdInventory);
}
