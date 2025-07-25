using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.DomainProduct;
using Poliedro.Eds.Domain.Product.Entities;
using System.Net;

namespace Poliedro.Eds.Application.Product.Commands.CreateProduct;
    public class CreateProductCommandHandler(
        IProductCreateProduct ProductDomainProduct,
        IMapper mapper,
        IValidator<CreateProductRequestDto> validator
        ) : IRequestHandler<CreateProductCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var ProductEntity = mapper.Map<ProductEntity>(request.Request);
                var result = await ProductDomainProduct.CreateAsync(ProductEntity);
                if (!result.IsSuccess)
                    return result.Error!;

                return result.Value!;
        }
    }