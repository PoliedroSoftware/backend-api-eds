using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Provider.Queries.GetProviderById;
public class GetProviderIdQueryValidator : AbstractValidator<GetProviderByIdQuery>
{
    //public GetProviderIdQueryValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Id)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
    //}
}