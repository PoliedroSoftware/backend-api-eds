using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.ShoppingProductInventory.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProductInventory.DomainShoppingProductInventory;
using System.Net;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Queries.GetShoppingProductInventoryById
{
    public class GetShoppingProductInventoryByIdQueryHandler(
        IShoppingProductInventoryGetById shoppingProductInventoryDomainService,
        IMapper mapper,
        IValidator<GetShoppingProductInventoryByIdQuery> validator
        ) : IRequestHandler<GetShoppingProductInventoryByIdQuery, Result<ShoppingProductInventoryDto, Error>>
    {
        public async Task<Result<ShoppingProductInventoryDto, Error>> Handle(GetShoppingProductInventoryByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<ShoppingProductInventoryDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await shoppingProductInventoryDomainService.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<ShoppingProductInventoryDto>(result.Value);

        }
    }
}
