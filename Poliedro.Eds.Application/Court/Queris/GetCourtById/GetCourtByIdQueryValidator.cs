using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Court.Queris.GetCourtById
{
    public class GetCourtByIdQueryValidator : AbstractValidator<GetCourtByIdQuery>
    {
        //public GetCourtByIdQueryValidator(ITranslationService translationService)
        //{
        //     RuleFor(x => x.Id)
        //    .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult()) 
        //    .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
        //}
    }
}
