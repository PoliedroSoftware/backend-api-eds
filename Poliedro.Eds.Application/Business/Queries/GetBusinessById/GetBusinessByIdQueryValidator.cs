using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Business.Queries.GetBusinessById;

public class GetBusinessIdQueryValidator : AbstractValidator<GetBusinessByIdQuery>
{
    //public GetBusinessIdQueryValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Id)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
    //}
}

