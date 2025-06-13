using MediatR;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Application.ShoppingProductInventory.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Queries.GellAllShoppingProductInventory
{
    public record GellAllShoppingProductInventoryQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<ShoppingProductInventoryDto>>;
}
