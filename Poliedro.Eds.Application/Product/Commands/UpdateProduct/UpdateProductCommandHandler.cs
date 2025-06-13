using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.DomainProduct;
using Poliedro.Eds.Domain.Product.Entities;

namespace Poliedro.Eds.Application.Product.Commands.UpdateProduct;

    public class UpdateProductCommandHandler(
        IProductUpdateProduct ProductDomainProduct,
        IMapper mapper
    ) : IRequestHandler<UpdateProductCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var ProductEntity = mapper.Map<ProductEntity>(request);
            var result = await ProductDomainProduct.UpdateAsync(ProductEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }