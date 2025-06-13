using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;

namespace Poliedro.Eds.Application.ShoppingProduct.Queries.GetShoppingProductById;
    public class GetShoppingProductByIdQueryHandler(
        IShoppingProductGetByIdShoppingProduct shoppingProductDomainService,
        IMapper mapper)
        : IRequestHandler<GetShoppingProductByIdQuery, Result<ShoppingProductDto, Error>>
    {
        public async Task<Result<ShoppingProductDto, Error>> Handle(GetShoppingProductByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await shoppingProductDomainService.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<ShoppingProductDto>(result.Value);
        }
    }

