using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.ProductType.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductType.DomainProductType;

namespace Poliedro.Eds.Application.ProductType.Queries.GetProductTypeById
{
    public class GetProductTypeByIdQueryHandler(
        IProductTypeGetByIdProductType ProductTypeDomainProductType,
        IMapper mapper,
        IValidator<GetProductTypeByIdQuery> validator)
        : IRequestHandler<GetProductTypeByIdQuery, Result<ProductTypeDto, Error>>
    {
        public async Task<Result<ProductTypeDto, Error>> Handle(GetProductTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<ProductTypeDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await ProductTypeDomainProductType.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<ProductTypeDto>(result.Value);
        }
    }
}
