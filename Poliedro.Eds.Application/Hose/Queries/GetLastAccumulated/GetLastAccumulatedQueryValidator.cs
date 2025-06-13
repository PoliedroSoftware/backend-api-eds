using FluentValidation;
using Poliedro.Eds.Application.Hose.Queries.GetLastAccumulated;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Hose.Queries.GetHoseById
{
    public class GetLastAccumulatedQueryValidator : AbstractValidator<GetLastAccumulatedQuery>
    {
        //public GetLastAccumulatedQueryValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.idDispenser)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("idDispenserNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("idDispenserGreaterThan").GetAwaiter().GetResult());
        //    RuleFor(x => x.idHose)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("idHoseNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("idHoseGreaterThan").GetAwaiter().GetResult());
        //}
    }
}
