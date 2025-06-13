using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Category.Queries.GetCategoryById;

    public class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
    {
        //public GetCategoryByIdQueryValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Id)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult()); 
        //}
    }
