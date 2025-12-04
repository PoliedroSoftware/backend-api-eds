using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Poliedro.Eds.Application.Hose.Commands.CreateHose;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.PosOfSale.Commands.CreatePosOfSale;

public class CreatePosOfSaleCommandValidator : AbstractValidator<CreatePosOfSaleRequestDto>
{
    public CreatePosOfSaleCommandValidator(IRedisService redisService)
    {
        RuleFor(x
            => x.Details)
            .NotNull()
            .WithMessage(redisService.GetValueFromCacheAsync("DetailsNotNull").GetAwaiter().GetResult()
                      ?? "La lista de detalles no puede ser nula")
            .Must(d => d.Any())
            .WithMessage(redisService.GetValueFromCacheAsync("DetailsNotEmpty").GetAwaiter().GetResult()
                      ?? "Debe existir al menos un detalle");
    }
}
