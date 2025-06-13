using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;

namespace Poliedro.Eds.Application.ShoppingProduct.Commands.UpdateShoppingProduct;
    public class UpdateShoppingProductCommandHandler(
        IShoppingProductUpdateShoppingProduct shoppingProductDomainShoppingProduct,
        IMapper mapper
   ) : IRequestHandler<UpdateShoppingProductCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateShoppingProductCommand request, CancellationToken cancellationToken)
        {
            var shoppingProductEntity = mapper.Map<ShoppingProductEntity>(request);
            var result = await shoppingProductDomainShoppingProduct.UpdateAsync(shoppingProductEntity);

            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }

