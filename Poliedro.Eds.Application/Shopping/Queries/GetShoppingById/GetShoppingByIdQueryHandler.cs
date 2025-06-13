using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Shopping.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Shopping.DomainShopping;

namespace Poliedro.Eds.Application.Shopping.Queries.GetShoppingById;
    public class GetShoppingByIdQueryHandler(
        IShoppingGetByIdShopping shoppingDomainService,
        IMapper mapper)
        : IRequestHandler<GetShoppingByIdQuery, Result<ShoppingDto, Error>>
    {
        public async Task<Result<ShoppingDto, Error>> Handle(GetShoppingByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await shoppingDomainService.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<ShoppingDto>(result.Value);
        }
    }

