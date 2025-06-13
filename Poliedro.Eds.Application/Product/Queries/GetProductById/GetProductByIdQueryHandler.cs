using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.DomainProduct;

namespace Poliedro.Eds.Application.Product.Queries.GetProductById
{
    public class GetProductByIdQueryHandler(
        IProductGetByIdProduct ProductDomainProduct,
        IMapper mapper)
        : IRequestHandler<GetProductByIdQuery, Result<ProductDto, Error>>
    {
        public async Task<Result<ProductDto, Error>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await ProductDomainProduct.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<ProductDto>(result.Value);
        }
    }
}
