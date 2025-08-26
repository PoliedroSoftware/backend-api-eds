using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Enums;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Inventory.Entities;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Domain.Shopping.DomainShopping;
using Poliedro.Eds.Domain.Shopping.Entities;

namespace Poliedro.Eds.Application.Shopping.Commands.CreateShopping;

public class CreateShoppingCommandHandler(
    IShoppingTransactionalService shoppingTransactionalService,
    IMapper mapper,
    IValidator<CreateShoppingRequestDto> validator
) : IRequestHandler<CreateShoppingCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateShoppingCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var shoppingEntity = mapper.Map<ShoppingEntity>(request.Request);

        shoppingEntity.ShoppingInventory = new InventoryEntity
        {
            Date = DateOnly.FromDateTime(shoppingEntity.Date),
            ReferenceType = ReferenceType.Shopping,
        };

        var productsToUpdatePrice = request.Request.ShoppingProducts
            .Where(sp => sp.SellPrice.HasValue)
            .Select(sp => new ProductEntity
            {
                IdProduct = sp.IdProduct,
                SellPrice = sp.SellPrice.Value
            })
            .ToList();

        return await shoppingTransactionalService.ExecuteShoppingTransactionAsync(
            shoppingEntity,
            productsToUpdatePrice);
    }
}
