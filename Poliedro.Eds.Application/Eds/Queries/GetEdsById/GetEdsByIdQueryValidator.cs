using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Eds.Queries.GetEdsById;

public class GetEdsyIdQueryValidator : AbstractValidator<GetEdsByIdQuery>
{
    //public GetEdsyIdQueryValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Id)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdNotGreaterThan").GetAwaiter().GetResult());
    //}
}