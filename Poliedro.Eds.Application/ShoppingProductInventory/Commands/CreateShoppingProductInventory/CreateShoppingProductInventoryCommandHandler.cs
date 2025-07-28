using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProductInventory.DomainShoppingProductInventory;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Commands.CreateShoppingProductInventory
{
    public class CreateShoppingProductInventoryCommandHandler(
        IShoppingCreateShoppingProductInventory shoppingProductDomainService,
        IMapper mapper,
        IValidator<CreateShoppingProductInventoryRequestDto> validator
        ) : IRequestHandler<CreateShoppingProductInventoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateShoppingProductInventoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var shoppingProductEntity = mapper.Map<ShoppingProductInventoryEntity>(request.Request);
            var result = await shoppingProductDomainService.CreateAsync(shoppingProductEntity);
            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;

        }
    }
}
