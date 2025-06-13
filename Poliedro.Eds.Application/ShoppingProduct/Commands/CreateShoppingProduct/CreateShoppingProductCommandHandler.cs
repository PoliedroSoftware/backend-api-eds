using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;

namespace Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;
public class CreateShoppingProductCommandHandler(
        IShoppingProductCreateShoppingProduct shoppingProductDomainService,
        IMapper mapper) : IRequestHandler<CreateShoppingProductCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateShoppingProductCommand request, CancellationToken cancellationToken)
        {
        var shoppingProductEntity = mapper.Map<ShoppingProductEntity>(request.Request);
        var result = await shoppingProductDomainService.CreateAsync(shoppingProductEntity);
            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }





