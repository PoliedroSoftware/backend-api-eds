using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Island.Commands.CreateIsland;

public class CreateIslandCommandValidator : AbstractValidator<CreateIslandRequestDto>
{
    //public CreateIslandCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Description)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("NameNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("NameNotEmpty").GetAwaiter().GetResult());
    //}
}
