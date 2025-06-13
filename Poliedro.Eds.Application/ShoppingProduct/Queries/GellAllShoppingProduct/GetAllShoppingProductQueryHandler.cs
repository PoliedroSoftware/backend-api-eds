using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;

namespace Poliedro.Eds.Application.ShoppingProduct.Queries.GellAllShoppingProduct;
public class GetAllShoppingProductQueryHandler
(
    IShoppingProductGetAllShoppingProduct shoppingProductDomainService,
    IMapper mapper)
    : IRequestHandler<GellAllShoppingProductQuery, IEnumerable<ShoppingProductDto>>
{
    public async Task<IEnumerable<ShoppingProductDto>> Handle(GellAllShoppingProductQuery request, CancellationToken cancellationToken)
    {
        var result = await shoppingProductDomainService.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<ShoppingProductDto>>(result);
    }
}


