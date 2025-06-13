using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.EdsTank.Commands.UpdateEdsTank;

    public class UpdateEdsTankCommandValidator : AbstractValidator<UpdateEdsTankCommand>
    {
        //public UpdateEdsTankCommandValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.IdEds)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdEdsNotNull").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdEdsNotEmpty").GetAwaiter().GetResult());
            
        //    RuleFor(x => x.IdTank)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdTank)NotNull").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdTankNotEmpty)").GetAwaiter().GetResult());
        //}
    }