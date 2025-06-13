using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Capacity.Commands.UpdateCapacity;

public class UpdateCapacityCommandValidator : AbstractValidator<UpdateCapacityCommand>
{
    //public UpdateCapacityCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.IdCapacity).NotNull().GreaterThan(0)
    //         .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdCapacityGreaterThan").GetAwaiter().GetResult());

    //    RuleFor(x => x.Code)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("CodeNotNull").GetAwaiter().GetResult()) 
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("CodeNotEmpty").GetAwaiter().GetResult()); 

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
}