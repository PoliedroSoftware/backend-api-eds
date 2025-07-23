using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.FileUploadS3.Command;

public class UploadFileValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileValidator(IRedisService redisService)
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("FileNotNull").GetAwaiter().GetResult())
            .Must(f => f.Length > 0).WithMessage(redisService.GetValueFromCacheAsync("FileMust").GetAwaiter().GetResult());
    }
}
