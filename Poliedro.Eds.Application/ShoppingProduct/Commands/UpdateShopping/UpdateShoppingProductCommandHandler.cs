using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;

namespace Poliedro.Eds.Application.ShoppingProduct.Commands.UpdateShoppingProduct;

public class UpdateShoppingProductCommandHandler(
    IShoppingProductUpdateShoppingProduct shoppingProductDomainShoppingProduct,
    IMapper mapper,
    IValidator<UpdateShoppingProductCommand> validator
    ) : IRequestHandler<UpdateShoppingProductCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(UpdateShoppingProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var shoppingProductEntity = mapper.Map<ShoppingProductEntity>(request);
        var result = await shoppingProductDomainShoppingProduct.UpdateAsync(shoppingProductEntity);

        if (!result.IsSuccess)
            return result.Error!;
        return result.Value!;
    }
}

