using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Hose.Commands.CreateHose;

public class CreateHoseCommandValidator : AbstractValidator<CreateHoseRequestDto>
{
    //public CreateHoseCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Number) 
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("NumberNotNull").GetAwaiter().GetResult())
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("NumberGreaterThan").GetAwaiter().GetResult());

    //    RuleFor(x => x.IdDispensers) 
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdDispensersNotNull").GetAwaiter().GetResult())
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdDispensersGreaterThan").GetAwaiter().GetResult());

    //    RuleFor(x => x.IdProductType) 
    //       .NotNull().WithMessage(translationService.GetTranslationByKey("IdProductTypeNotNull").GetAwaiter().GetResult())
    //       .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdProductTypeGreaterThan)").GetAwaiter().GetResult());
    //}
}
