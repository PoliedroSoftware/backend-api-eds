using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.FileUploadS3.Command;

public class DescribeImageValidator : AbstractValidator<DescribeImageCommand>
{
    public DescribeImageValidator(IRedisService redisService)
    {
        RuleFor(x => x.Image)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("ImageNotNull").GetAwaiter().GetResult())
            .Must(f => f.Length > 0).WithMessage(redisService.GetValueFromCacheAsync("ImageMust").GetAwaiter().GetResult())
            .Must(f => IsImageFile(f.ContentType)).WithMessage(redisService.GetValueFromCacheAsync("ImageType").GetAwaiter().GetResult());
    }

    private static bool IsImageFile(string contentType)
    {
        return contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
    }
}