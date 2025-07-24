using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.DomainProduct;
using System.Net;

namespace Poliedro.Eds.Application.Product.Queries.GetProductById
{
    public class GetProductByIdQueryHandler(
        IProductGetByIdProduct ProductDomainProduct,
        IMapper mapper,
        IValidator<GetProductByIdQuery> validator)
        : IRequestHandler<GetProductByIdQuery, Result<ProductDto, Error>>
    {
        public async Task<Result<ProductDto, Error>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<ProductDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await ProductDomainProduct.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<ProductDto>(result.Value);
        }
    }
}
