using FluentValidation;
using Poliedro.Eds.Application.DispenserType.Commands.CreateDispenserType;
using Poliedro.Eds.Application.Ports.Translations;


namespace Poliedro.Eds.Application.Server.DispenserType.CreateDispenserType;

public class CreateDispenserTypeCommandValidator : AbstractValidator<CreateDispenserTypeRequestDto>
{
    //public CreateDispenserTypeCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Description)
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("DescriptionNotEmpty").GetAwaiter().GetResult())
    //        .MaximumLength(50).WithMessage(translationService.GetTranslationByKey("DescriptionMaximumLength").GetAwaiter().GetResult());
    //}
}
