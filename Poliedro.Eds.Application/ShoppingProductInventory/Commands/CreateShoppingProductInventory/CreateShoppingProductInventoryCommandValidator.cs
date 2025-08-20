using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Commands.CreateShoppingProductInventory
{
    public class CreateShoppingProductInventoryCommandValidator : AbstractValidator<CreateShoppingProductInventoryRequestDto>
    {
        public CreateShoppingProductInventoryCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.IdShoppingProductInventory)
           .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdShoppingProductInventoryGreaterThan").GetAwaiter().GetResult())
           .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdShoppingProductInventoryNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.IdShoppingProduct)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdShoppingProductGreaterThan").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdShoppingProductNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.IdInventory)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdInventoryGreaterThan").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdInventoryNotEmpty").GetAwaiter().GetResult());
        }
    }
}
