using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Domain.ShoppingProductInventory.DomainShoppingProductInventory;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Commands.CreateShoppingProductInventory
{
    public class CreateShoppingProductInventoryCommandHandler(IShoppingCreateShoppingProductInventory shoppingProductDomainService, IMapper mapper) : IRequestHandler<CreateShoppingProductInventoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateShoppingProductInventoryCommand request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();


            var shoppingProductEntity = mapper.Map<ShoppingProductInventoryEntity>(request.Request);
            var result = await shoppingProductDomainService.CreateAsync(shoppingProductEntity);
            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;

        }
    }
}
