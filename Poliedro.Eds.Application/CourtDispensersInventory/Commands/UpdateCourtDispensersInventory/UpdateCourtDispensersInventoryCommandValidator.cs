using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Commands.UpdateCourtDispensersInventory
{
    public class UpdateCourtDispensersInventoryCommandValidator : AbstractValidator<UpdateCourtDispensersInventoryCommand>
    {
        //public UpdateCourtDispensersInventoryCommandValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.IdCourtdDispensers)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdCourtdDispensersNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdCourtdDispensersGreaterThan").GetAwaiter().GetResult());

        //    RuleFor(x => x.IdInventory)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdInventoryNotNull").GetAwaiter().GetResult()) 
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdInventoryGreaterThan").GetAwaiter().GetResult()); 
        //}
    }
}
