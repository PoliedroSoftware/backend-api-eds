using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.ProductType.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductType.DomainProductType;

namespace Poliedro.Eds.Application.ProductType.Queries.GetProductTypeById
{
    public class GetProductTypeByIdQueryHandler(
        IProductTypeGetByIdProductType ProductTypeDomainProductType,
        IMapper mapper)
        : IRequestHandler<GetProductTypeByIdQuery, Result<ProductTypeDto, Error>>
    {
        public async Task<Result<ProductTypeDto, Error>> Handle(GetProductTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await ProductTypeDomainProductType.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<ProductTypeDto>(result.Value);
        }
    }
}
