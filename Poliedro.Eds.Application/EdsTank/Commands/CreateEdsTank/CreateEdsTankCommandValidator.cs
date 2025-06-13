using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.EdsTank.Commands.CreateEdsTank;

public class CreateEdsTankCommandValidator : AbstractValidator<CreateEdsTankRequestDto>
{
    //public CreateEdsTankCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.IdEds)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdEdsNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdEdsNotEmpty").GetAwaiter().GetResult());
        
    //    RuleFor(x => x.IdTank)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdTankNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdTankNotEmpty").GetAwaiter().GetResult());
    //}
}
