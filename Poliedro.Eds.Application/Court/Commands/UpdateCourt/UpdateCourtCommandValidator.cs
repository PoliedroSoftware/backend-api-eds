using FluentValidation;
using Poliedro.Eds.Application.Court.Commands.CreateCourt;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Court.Commands.UpdateCourt
{
    public class UpdateCourtCommandValidator : AbstractValidator<UpdateCourtCommand>
    {
    //    public UpdateCourtCommandValidator(ITranslationService translationService)
    //    {
    //        RuleFor(x => x.IdCourt)
    //            .NotNull().WithMessage(translationService.GetTranslationByKey("IdCourtNotNull").GetAwaiter().GetResult())
    //            .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdCourtNotEmpty").GetAwaiter().GetResult()); 

    //        RuleFor(x => x.DateStarttime)
    //            .NotNull().WithMessage(translationService.GetTranslationByKey("DateStrattimeNotNull").GetAwaiter().GetResult()) 
    //            .NotEmpty().WithMessage(translationService.GetTranslationByKey("DateNotEmpty").GetAwaiter().GetResult());

    //        RuleFor(x => x.Starttime)
    //            .NotNull().WithMessage(translationService.GetTranslationByKey("StarttimeNotNull").GetAwaiter().GetResult()) 
    //            .NotEmpty().WithMessage(translationService.GetTranslationByKey("StarttimeNotEmpty").GetAwaiter().GetResult());

    //        RuleFor(x => x.DateEndtime)
    //            .NotNull().WithMessage(translationService.GetTranslationByKey("DateEndtimeNotNull").GetAwaiter().GetResult())
    //            .NotEmpty().WithMessage(translationService.GetTranslationByKey("DateNotEmpty").GetAwaiter().GetResult());

    //        RuleFor(x => x.Endtime)
    //            .NotNull().WithMessage(translationService.GetTranslationByKey("EndtimeNotNull").GetAwaiter().GetResult()) 
    //            .NotEmpty().WithMessage(translationService.GetTranslationByKey("EndtimeNotEmpty").GetAwaiter().GetResult());
    //    }
    //    public class GetServerByIdCommandValidator : AbstractValidator<GetCourtByIdCommand>
    //    {
    //        public GetServerByIdCommandValidator(ITranslationService translationService)
    //        {
    //            RuleFor(x => x.Id)
    //                .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult()); 
    //        }
    //    }
    }
}
