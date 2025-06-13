using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Expenditures.Queries.GetExpendituresById;
    public class GetExpendituresByIdQueryValidator : AbstractValidator<GetExpendituresByIdQuery>
    {
        //public GetExpendituresByIdQueryValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Id)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
        //}
    }
