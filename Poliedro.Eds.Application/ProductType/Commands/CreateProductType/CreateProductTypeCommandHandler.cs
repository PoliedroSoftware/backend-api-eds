using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductType.DomainProductType;
using Poliedro.Eds.Domain.ProductType.Entities;

namespace Poliedro.Eds.Application.ProductType.Commands.CreateProductType
{
    public class CreateProductTypeCommandHandler(
        IProductTypeCreateProductType productTypeDomainProductType,
        IMapper mapper) : IRequestHandler<CreateProductTypeCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateProductTypeCommand request, CancellationToken cancellationToken)
        {
            var ProductTypeEntity = mapper.Map<ProductTypeEntity>(request.Request);
            var result = await productTypeDomainProductType.CreateAsync(ProductTypeEntity);
            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }
}




