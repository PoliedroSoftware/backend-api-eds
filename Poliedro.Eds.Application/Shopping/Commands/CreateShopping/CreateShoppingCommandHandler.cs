using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Product.Services;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Inventory.Entities;
using Poliedro.Eds.Domain.Shopping.DomainShopping;
using Poliedro.Eds.Domain.Shopping.Entities;

namespace Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
    public class CreateShoppingCommandHandler(
        IShoppingCreateShopping shoppingDomainService,
        IMapper mapper,
        IProductPriceUpdateService productPriceUpdateService
        ) : IRequestHandler<CreateShoppingCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateShoppingCommand request, CancellationToken cancellationToken)
        {

        var shoppingEntity = mapper.Map<ShoppingEntity>(request.Request);


        //if (request.Request.SellPriceProducts is { } sellPriceProducts && sellPriceProducts.Any())
        //{
        //    var products = mapper.Map<IEnumerable<ProductEntity>>(sellPriceProducts);
        //    var priceUpdateResult = await productPriceUpdateService.UpdatePricesAsync(products);
        //    if (!priceUpdateResult.IsSuccess)
        //        return priceUpdateResult.Error!;
        //}

        shoppingEntity.ShoppingInventory = new InventoryEntity
        {
            Date = DateOnly.FromDateTime(shoppingEntity.Date),
            ReferenceType = "shopping",
        };

        
        var result = await shoppingDomainService.CreateAsync(shoppingEntity);
            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }





