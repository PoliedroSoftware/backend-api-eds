using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Business.Commands.UpdateBusiness;

public class UpdateBusinessCommandValidator : AbstractValidator<UpdateBusinessCommand>
{
    //public UpdateBusinessCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.IdBusiness).NotNull().GreaterThan(0) 
    //         .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdBusinessGreaterThan").GetAwaiter().GetResult());

    //    RuleFor(x => x.Name)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("NameNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("NameNotEmpty").GetAwaiter().GetResult());

    //}
}