using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.CompartimentCapacity.Queries.GetCompartimentCapacityById;
    public class GetCompartimentCapacityByIdQueryValidator : AbstractValidator<GetCompartimentCapacityByIdQuery>
    {
        //public GetCompartimentCapacityByIdQueryValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Id)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
        //}
    }