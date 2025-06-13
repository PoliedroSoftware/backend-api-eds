using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.DispenserType.Queries.GetDispenserTypeById
{
    public class GetDispenserTypeByIdQueryValidator : AbstractValidator<GetDispenserTypeByIdQuery>
    {
        //public GetDispenserTypeByIdQueryValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Id)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
        //}
    }
}