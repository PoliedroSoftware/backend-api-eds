using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
namespace Poliedro.Eds.Application.DispenserType.Commands.UpdateDispenserType
{
    public class UpdateDispenserTypeCommandValidator : AbstractValidator<UpdateDispenserTypeCommand>
    {
        public UpdateDispenserTypeCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.IdType)
    .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdTypeNotNull").GetAwaiter().GetResult()) // Asegura que no sea null
    .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdTypeGreaterThan").GetAwaiter().GetResult()); // Debe ser mayor que 0

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("DescriptionNotEmpty").GetAwaiter().GetResult())
                .MaximumLength(50).WithMessage(redisService.GetValueFromCacheAsync("DescriptionMaximumLength").GetAwaiter().GetResult());
        }
    }
}
