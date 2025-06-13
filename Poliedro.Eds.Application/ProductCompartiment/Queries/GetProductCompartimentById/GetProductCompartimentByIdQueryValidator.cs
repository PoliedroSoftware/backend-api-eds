using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.ProductCompartiment.Queries.GetProductCompartimentById
{
    public class GetProductCompartimentByIdQueryValidator : AbstractValidator<GetProductCompartimentByIdQuery>
    {
        //public GetProductCompartimentByIdQueryValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Id)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
        //}
    }
}