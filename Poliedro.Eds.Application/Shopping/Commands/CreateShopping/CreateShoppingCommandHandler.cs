using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Shopping.DomainShopping;
using Poliedro.Eds.Domain.Shopping.Entities;

namespace Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
    public class CreateShoppingCommandHandler(
        IShoppingCreateShopping shoppingDomainService,
        IMapper mapper) : IRequestHandler<CreateShoppingCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateShoppingCommand request, CancellationToken cancellationToken)
        {
        var shoppingEntity = mapper.Map<ShoppingEntity>(request.Request);
        var result = await shoppingDomainService.CreateAsync(shoppingEntity);
            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }





