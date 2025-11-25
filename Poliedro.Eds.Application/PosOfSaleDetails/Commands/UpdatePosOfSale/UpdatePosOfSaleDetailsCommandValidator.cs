using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Poliedro.Eds.Application.Hose.Commands.UpdateHose;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Commands.UpdatePosOfSale
{
    public class UpdatePosOfSaleDetailsCommandValidator : AbstractValidator<UpdatePosOfSaleDetailsCommand>
    {
        public UpdatePosOfSaleDetailsCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.IdDetail)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.PosId)
           .NotNull().WithMessage(redisService.GetValueFromCacheAsync("PosIdNotNull").GetAwaiter().GetResult())
           .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("PosIdNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.ProductCode)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("ProductCodeNotNull").GetAwaiter().GetResult());

            RuleFor(x => x.ProductName)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("ProductNameNotNull").GetAwaiter().GetResult());
        }
    }
}
