using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.ShoppingProduct.Queries.GetShoppingProductById;
using Poliedro.Eds.Application.ShoppingProductInventory.Queries.GetShoppingProductInventoryById;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Queries.GetShoppingProductInventoryById
{
    public class GetShoppingProductInventoryByIdQueryValidator : AbstractValidator<GetShoppingProductInventoryByIdQuery>
    {
        public GetShoppingProductInventoryByIdQueryValidator(IRedisService redisService)
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdNotNull").GetAwaiter().GetResult())
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdGreaterThan").GetAwaiter().GetResult());
        }
    }
}
