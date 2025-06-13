using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Tank.Commands.CreateTank;

public class CreateTankCommandValidator : AbstractValidator<CreateTankRequestDto>
{
    //public CreateTankCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Number)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("NumberNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("NumberNotEmpty").GetAwaiter().GetResult());

    //    RuleFor(x => x.Compartment)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("CompartmentNotNull").GetAwaiter().GetResult())
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("CompartmentGreaterThan").GetAwaiter().GetResult());

    //    RuleFor(x => x.Ability)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("AbilityNotNull").GetAwaiter().GetResult())
    //        .GreaterThan(0.0).WithMessage(translationService.GetTranslationByKey("AbilityGreaterThan").GetAwaiter().GetResult());

    //    RuleFor(x => x.Stock)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("StockNotNull").GetAwaiter().GetResult())
    //        .GreaterThan(0.0).WithMessage(translationService.GetTranslationByKey("StockGreaterThan").GetAwaiter().GetResult());

    //}
}
