using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductType.DomainProductType;
using Poliedro.Eds.Domain.ProductType.Entities;

namespace Poliedro.Eds.Application.ProductType.Commands.UpdateProductType
{
    public class UpdateProductTypeCommandHandler(
        IProductTypeUpdateProductType ProductTypeDomainProductType,
        IMapper mapper
    ) : IRequestHandler<UpdateProductTypeCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateProductTypeCommand request, CancellationToken cancellationToken)
        {
            var ProductTypeEntity = mapper.Map<ProductTypeEntity>(request);
            var result = await ProductTypeDomainProductType.UpdateAsync(ProductTypeEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }
}