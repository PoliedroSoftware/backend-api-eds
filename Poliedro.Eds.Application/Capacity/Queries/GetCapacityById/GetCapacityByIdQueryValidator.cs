using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Capacity.Queries.GetCapacityById;
public class GetCapacityIdQueryValidator : AbstractValidator<GetCapacityByIdQuery>
{
    //public GetCapacityIdQueryValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Id)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("NotNull").GetAwaiter().GetResult())
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("GreaterThan").GetAwaiter().GetResult());
    //}
}