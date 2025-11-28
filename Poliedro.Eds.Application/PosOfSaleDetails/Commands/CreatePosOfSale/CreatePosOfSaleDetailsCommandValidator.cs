using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Poliedro.Eds.Application.Hose.Commands.CreateHose;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Commands.CreatePosOfSale;

public class CreatePosOfSaleDetailsCommandValidator : AbstractValidator<CreatePosOfSaleDetailsRequestDto>
{
    public CreatePosOfSaleDetailsCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.PosId)
       .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdNotNull").GetAwaiter().GetResult() ?? "El PosId no puede ser nulo")
        .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdGreaterThan").GetAwaiter().GetResult() ?? "El PosId debe ser mayor a 0");

        RuleFor(x => x.ProductCode)
        .NotNull().WithMessage(redisService.GetValueFromCacheAsync("ProductCodeNotNull").GetAwaiter().GetResult() ?? "El codigo del producto no puede ser nulo");

        RuleFor(x => x.ProductName)
        .NotNull().WithMessage(redisService.GetValueFromCacheAsync("ProductNameNotNull").GetAwaiter().GetResult() ?? "El nombre del producto no puede ser nulo");
    }
}
