using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Commands.UpdateShoppingProductInventory
{
    public class UpdateShoppingProductInventoryCommandValidator :  AbstractValidator<UpdateShoppingProductInventoryCommand>
    {
        public UpdateShoppingProductInventoryCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.IdShopping)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdShoppingGreaterThan").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdShoppingNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.IdInventory)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdInventoryGreaterThan").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdInventoryNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.IdShoppingProduct)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdShoppingProductGreaterThan").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdShoppingProductNotEmpty").GetAwaiter().GetResult());
        }
    }
}
