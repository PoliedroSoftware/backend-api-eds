using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.HoseHistory.Commands.UpdateHoseHistory
{
    public class UpdateHoseHistoryCommandValidator : AbstractValidator<UpdateHoseHistoryCommand>
    {
    //    public UpdateHoseHistoryCommandValidator(ITranslationService translationService)
    //    {
    //        RuleFor(x => x.IdHoseHistory)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("NotNullIdHoseHistory").GetAwaiter().GetResult())
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("GreaterThanIdHoseHistory").GetAwaiter().GetResult());

    //        RuleFor(x => x.IdHose)
    //            .NotNull().WithMessage(translationService.GetTranslationByKey("IdHoseNotNull").GetAwaiter().GetResult())
    //            .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdHoseGreaterThan").GetAwaiter().GetResult());

    //        RuleFor(x => x.AccumulatedGallons)
    //            .NotNull().WithMessage(translationService.GetTranslationByKey("AccumulatedGallonsNotNull").GetAwaiter().GetResult())
    //            .GreaterThan(0.0).WithMessage(translationService.GetTranslationByKey("AccumulatedGallonsGreaterThan").GetAwaiter().GetResult());

    //        RuleFor(x => x.AccumulatedAmount)
    //            .NotNull().WithMessage(translationService.GetTranslationByKey("AccumulatedAmountNotNull").GetAwaiter().GetResult())
    //            .GreaterThan(0.0).WithMessage(translationService.GetTranslationByKey("AccumulatedAmountGreaterThan").GetAwaiter().GetResult());

    //        RuleFor(x => x.IdDispensers)
    //           .NotNull().WithMessage(translationService.GetTranslationByKey("IdDispensersNotNull").GetAwaiter().GetResult())
    //           .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdDispensersGreaterThan").GetAwaiter().GetResult());

    //        RuleFor(x => x.Date)
    //           .NotNull().WithMessage(translationService.GetTranslationByKey("DateNotNull").GetAwaiter().GetResult())
    //           .GreaterThan(DateTime.MinValue).WithMessage(translationService.GetTranslationByKey("DateGreaterThan").GetAwaiter().GetResult());
    //    }
    }
}
