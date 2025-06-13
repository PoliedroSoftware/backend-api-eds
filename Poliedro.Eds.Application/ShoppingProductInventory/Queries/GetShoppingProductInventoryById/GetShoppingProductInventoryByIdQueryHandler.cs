using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Application.ShoppingProduct.Queries.GetShoppingProductById;
using Poliedro.Eds.Application.ShoppingProductInventory.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProductInventory.DomainShoppingProductInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Queries.GetShoppingProductInventoryById
{
    public class GetShoppingProductInventoryByIdQueryHandler(IShoppingProductInventoryGetById shoppingProductInventoryDomainService, IMapper mapper) : IRequestHandler<GetShoppingProductInventoryByIdQuery, Result<ShoppingProductInventoryDto, Error>>
    {
        public async Task<Result<ShoppingProductInventoryDto, Error>> Handle(GetShoppingProductInventoryByIdQuery request, CancellationToken cancellationToken)
        {

            var result = await shoppingProductInventoryDomainService.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<ShoppingProductInventoryDto>(result.Value);

        }
    }
}
