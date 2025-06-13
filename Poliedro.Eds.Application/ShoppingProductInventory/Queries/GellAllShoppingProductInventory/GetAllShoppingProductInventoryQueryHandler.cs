using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Application.ShoppingProduct.Queries.GellAllShoppingProduct;
using Poliedro.Eds.Application.ShoppingProductInventory.Dtos;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProductInventory.DomainShoppingProductInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Queries.GellAllShoppingProductInventory
{
    public class GetAllShoppingProductInventoryQueryHandler(IShoppingProductInventoryGetAll shoppingProductDomainService, IMapper mapper) : IRequestHandler<GellAllShoppingProductInventoryQuery, IEnumerable<ShoppingProductInventoryDto>>
    {
        public async Task<IEnumerable<ShoppingProductInventoryDto>> Handle(GellAllShoppingProductInventoryQuery request, CancellationToken cancellationToken)
        {
            var result = await shoppingProductDomainService.GetAllAsync(request.PaginationParams);
            return mapper.Map<List<ShoppingProductInventoryDto>>(result);
        }
    }
}
