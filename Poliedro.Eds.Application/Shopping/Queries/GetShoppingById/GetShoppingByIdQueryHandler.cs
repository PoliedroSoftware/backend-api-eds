using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Shopping.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Shopping.DomainShopping;

namespace Poliedro.Eds.Application.Shopping.Queries.GetShoppingById;

public class GetShoppingByIdQueryHandler(
    IShoppingGetByIdShopping shoppingDomainService,
    IMapper mapper,
    IValidator<GetShoppingByIdQuery> validator)
    : IRequestHandler<GetShoppingByIdQuery, Result<ShoppingDto, Error>>
{
    public async Task<Result<ShoppingDto, Error>> Handle(GetShoppingByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Result<ShoppingDto, Error>.Failure(
            Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
        }
        var result = await shoppingDomainService.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        return mapper.Map<ShoppingDto>(result.Value);
    }
}

