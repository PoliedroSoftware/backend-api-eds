using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Poliedro.Eds.Application.Hose.Commands.UpdateHose;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.PosOfSale.Commands.UpdatePosOfSale
{
    public class UpdatePosOfSaleCommandValidator : AbstractValidator<UpdatePosOfSaleCommand>
    {
        public UpdatePosOfSaleCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.IdPos)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.InvoiceNumber)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NumberNotNull").GetAwaiter().GetResult());
        }
    }
}
