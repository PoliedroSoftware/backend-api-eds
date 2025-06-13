using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Commands.CreateCourtDispensersInventory;

public class CreateCourtDispensersInventoryCommandValidator : AbstractValidator<CreateCourtDispensersInventoryRequestDto>
{
    //public CreateCourtDispensersInventoryCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.IdCourtdDispensers)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdCourtdDispensersNotNul").GetAwaiter().GetResult())
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdCourtdDispensersNotNul").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.IdInventory)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdInventoryNotNull").GetAwaiter().GetResult())
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdInventoryGreaterThan").GetAwaiter().GetResult());
    //}
}
