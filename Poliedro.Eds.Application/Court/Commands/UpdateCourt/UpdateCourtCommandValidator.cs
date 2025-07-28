using FluentValidation;
using Poliedro.Eds.Application.Court.Commands.CreateCourt;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Court.Commands.UpdateCourt
{
    public class UpdateCourtCommandValidator : AbstractValidator<UpdateCourtCommand>
    {
        public UpdateCourtCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.IdCourt)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdCourtNotNull").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdCourtNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.DateStarttime)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("DateStrattimeNotNull").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("DateNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.Starttime)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("StarttimeNotNull").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("StarttimeNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.DateEndtime)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("DateEndtimeNotNull").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("DateNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.Endtime)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("EndtimeNotNull").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("EndtimeNotEmpty").GetAwaiter().GetResult());
        }
        public class GetServerByIdCommandValidator : AbstractValidator<GetCourtByIdCommand>
        {
            public GetServerByIdCommandValidator(IRedisService redisService)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdGreaterThan").GetAwaiter().GetResult());
            }
        }
    }
}
