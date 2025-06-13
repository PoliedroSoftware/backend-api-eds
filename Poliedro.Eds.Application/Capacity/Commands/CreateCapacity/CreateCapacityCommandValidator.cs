using FluentValidation;
using Poliedro.Eds.Application.Capacity.Queries.GetCapacityById;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Capacity.Commands.CreateCapacity;

public class CreateCapacityCommandValidator : AbstractValidator<CreateCapacityRequestDto>
{
    //public CreateCapacityCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Code)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("CodeNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("CodeNotEmpty").GetAwaiter().GetResult())
    //        .NotEqual("string").WithMessage(translationService.GetTranslationByKey("CodeNotEqual").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.Height)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("HeightNotNull").GetAwaiter().GetResult())    
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("HeightNotEmpty").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.Gallon)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("GallonNotNull").GetAwaiter().GetResult()) 
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("GallonNotEmpty").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.Liters)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("LitersNotNull").GetAwaiter().GetResult()) 
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("LitersNotEmpty").GetAwaiter().GetResult());

    //}
    //public class GetCapacityByIdCommandValidator : AbstractValidator<GetCapacityByIdQuery>
    //{
    //    public GetCapacityByIdCommandValidator(ITranslationService translationService)
    //    {
    //        RuleFor(x => x.Id)
    //            .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
    //    }
    //}
}
