using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Compartiment.Queries.GetCompartimentById
{
    public class GetCompartimentByIdQueryValidator : AbstractValidator<GetCompartimentByIdQuery>
    {
        //public GetCompartimentByIdQueryValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Id)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult()); 
        //}
    }
}