using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Islander.Queries.GetIslanderById
{
    public class GetIslanderyIdQueryValidator : AbstractValidator<GetIslanderByIdQuery>
    {
        //public GetIslanderyIdQueryValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Id)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
        //}
    }
}
