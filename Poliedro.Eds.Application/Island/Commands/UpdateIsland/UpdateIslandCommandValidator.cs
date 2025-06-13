using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Island.Commands.UpdateIsland
{
    public class UpdateIslandCommandValidator : AbstractValidator<UpdateIslandCommand>
    {
        //public UpdateIslandCommandValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Description)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("NameNotNull").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("NameNotEmpty").GetAwaiter().GetResult());

        //    RuleFor(x => x.IdIsland).NotNull().GreaterThan(0)
        //         .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdEdsGreaterThan").GetAwaiter().GetResult());
        //}
    }
} 