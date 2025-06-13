using FluentValidation;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Memory;
using Poliedro.Eds.Application.Business.Queries.GetBusinessById;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Business.Commands.CreateBusiness;

public class CreateBusinessCommandValidator : AbstractValidator<CreateBusinessRequestDto>
{
    public CreateBusinessCommandValidator(IMemoryCache memoryCache, IRedisService redisService)
    {
     
        RuleFor(x => x.Name)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NameNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NameNotEmpty").GetAwaiter().GetResult())
            .NotEqual("string").WithMessage(redisService.GetValueFromCacheAsync("NameNotEqual").GetAwaiter().GetResult());

    }
    //public class GetBusinessByIdCommandValidator : AbstractValidator<GetBusinessByIdQuery>
    //{
    //    public GetBusinessByIdCommandValidator(ITranslationService translationService)
    //    {
    //        RuleFor(x => x.Id)
    //            .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
    //    }
    //}
}

                 