using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.HoseHistory.Queries.GetHoseHistoryById
{
    public class GetHoseHistoryIdQueryValidator : AbstractValidator<GetHoseHistoryByIdQuery>
    {
        //public GetHoseHistoryIdQueryValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Id)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
        //}
    }
}
