using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Tank.Queries.GetTankById
{
    public class GetTankIdQueryValidator : AbstractValidator<GetTankByIdQuery>
    {
        //public GetTankIdQueryValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Id)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
        //}
    }
}
