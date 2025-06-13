using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.FileUploadS3.Command;

public class UploadFileValidator : AbstractValidator<UploadFileCommand>
{
    //public UploadFileValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.File)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("FileNotNull").GetAwaiter().GetResult())
    //        .Must(f => f.Length > 0).WithMessage(translationService.GetTranslationByKey("FileMust").GetAwaiter().GetResult());
    //}
}
