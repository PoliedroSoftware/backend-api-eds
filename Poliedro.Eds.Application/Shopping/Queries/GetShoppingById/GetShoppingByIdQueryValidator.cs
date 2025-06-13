using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Shopping.Queries.GetShoppingById;

    public class GetShoppingByIdQueryValidator : AbstractValidator<GetShoppingByIdQuery>
    {
        //public GetShoppingByIdQueryValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Id)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
        //}
    }
