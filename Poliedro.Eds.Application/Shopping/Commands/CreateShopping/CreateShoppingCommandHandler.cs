using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Shopping.DomainShopping;
using Poliedro.Eds.Domain.Shopping.Entities;
using Poliedro.Eds.Application.Product.Services;
using Poliedro.Eds.Domain.Product.Entities;

namespace Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
    public class CreateShoppingCommandHandler(
        IShoppingCreateShopping shoppingDomainService,
        IMapper mapper,
        IProductPriceUpdateService productPriceUpdateService
        ) : IRequestHandler<CreateShoppingCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateShoppingCommand request, CancellationToken cancellationToken)
        {

        if (request.Request.SellPriceProducts is { } sellPriceProducts && sellPriceProducts.Any())
        {
            var products = mapper.Map<IEnumerable<ProductEntity>>(sellPriceProducts);
            var priceUpdateResult = await productPriceUpdateService.UpdatePricesAsync(products);
            if (!priceUpdateResult.IsSuccess)
                return priceUpdateResult.Error!;
        }

        var shoppingEntity = mapper.Map<ShoppingEntity>(request.Request);
        var result = await shoppingDomainService.CreateAsync(shoppingEntity);
            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }





