using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Shopping.DomainShopping;
using Poliedro.Eds.Domain.Shopping.Entities;

namespace Poliedro.Eds.Application.Shopping.Commands.UpdateShopping
{
    public class UpdateShoppingCommandHandler(
        IShoppingUpdateShopping shoppingDomainShopping,
        IMapper mapper
   ) : IRequestHandler<UpdateShoppingCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateShoppingCommand request, CancellationToken cancellationToken)
        {
            var shoppingEntity = mapper.Map<ShoppingEntity>(request);
            var result = await shoppingDomainShopping.UpdateAsync(shoppingEntity);

            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}
