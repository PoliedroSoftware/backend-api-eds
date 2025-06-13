using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.TypeOfCollection.Queries.GetTypeOfCollectionById;
    public class GetTypeOfCollectionByIdQueryValidator : AbstractValidator<GetTypeOfCollectionByIdQuery>
    {
        //public GetTypeOfCollectionByIdQueryValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Id)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
        //}
    }