using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Hose.Commands.UpdateHose
{
    public class UpdateHoseCommandValidator : AbstractValidator<UpdateHoseCommand>
    {
        //public UpdateHoseCommandValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Number)
        //    .NotNull().WithMessage(translationService.GetTranslationByKey("NumberNotNull").GetAwaiter().GetResult())
        //    .NotEmpty().WithMessage(translationService.GetTranslationByKey("NumberNotEmpty").GetAwaiter().GetResult());

        //    RuleFor(x => x.IdDispensers)  
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdDispensersNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdDispensersGreaterThan").GetAwaiter().GetResult());

        //    RuleFor(x => x.AccumulatedGallons)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("AccumulatedGallonsNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0.0).WithMessage(translationService.GetTranslationByKey("AccumulatedGallonsGreaterThan").GetAwaiter().GetResult());

        //    RuleFor(x => x.AccumulatedAmount)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("AccumulatedAmountNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0.0).WithMessage(translationService.GetTranslationByKey("AccumulatedAmountGreaterThan").GetAwaiter().GetResult());

        //    RuleFor(x => x.IdProductType)
        //       .NotNull().WithMessage(translationService.GetTranslationByKey("IdProductTypeNotNull").GetAwaiter().GetResult())
        //       .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdProductTypeGreaterThan").GetAwaiter().GetResult());
        //}
    }
}
