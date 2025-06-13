using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Queries.GetCourtDispensersInventoryById
{
    public class GetCourtDispensersInventoryIdQueryValidator : AbstractValidator<GetCourtDispensersInventoryByIdQuery>
    {
        //public GetCourtDispensersInventoryIdQueryValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Id)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
        //}
    }
}
