using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.DomainProduct;
using Poliedro.Eds.Domain.Product.Entities;

namespace Poliedro.Eds.Application.Product.Commands.CreateProduct;
    public class CreateProductCommandHandler(
        IProductCreateProduct ProductDomainProduct,
        IMapper mapper) : IRequestHandler<CreateProductCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var ProductEntity = mapper.Map<ProductEntity>(request.Request);
            var result = await ProductDomainProduct.CreateAsync(ProductEntity);
            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }





