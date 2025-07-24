using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using System.Net;

namespace Poliedro.Eds.Application.ShoppingProduct.Queries.GetShoppingProductById;
    public class GetShoppingProductByIdQueryHandler(
        IShoppingProductGetByIdShoppingProduct shoppingProductDomainService,
        IMapper mapper,
        IValidator<GetShoppingProductByIdQuery> validator)
        : IRequestHandler<GetShoppingProductByIdQuery, Result<ShoppingProductDto, Error>>
    {
        public async Task<Result<ShoppingProductDto, Error>> Handle(GetShoppingProductByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<ShoppingProductDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await shoppingProductDomainService.GetByIdAsync(request.Id);
                if (!result.IsSuccess)
                    return result.Error!;

                return mapper.Map<ShoppingProductDto>(result.Value);
        }
    }

