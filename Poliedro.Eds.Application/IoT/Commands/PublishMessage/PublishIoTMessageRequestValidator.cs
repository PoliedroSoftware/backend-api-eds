using FluentValidation;

namespace Poliedro.Eds.Application.IoT.Commands.PublishMessage;

public class PublishIoTMessageRequestValidator : AbstractValidator<PublishIoTMessageRequest>
{
    public PublishIoTMessageRequestValidator()
    {
        RuleFor(x => x.Message)
            .NotNull()
            .WithMessage("Message is required");

        RuleFor(x => x.Message.Input1)
            .NotEmpty()
            .WithMessage("Input1 is required")
            .MaximumLength(255)
            .WithMessage("Input1 must not exceed 255 characters");

        RuleFor(x => x.Message.Input2)
            .NotEmpty()
            .WithMessage("Input2 is required")
            .MaximumLength(255)
            .WithMessage("Input2 must not exceed 255 characters");

        RuleFor(x => x.Message.Output1)
            .NotEmpty()
            .WithMessage("Output1 is required")
            .MaximumLength(255)
            .WithMessage("Output1 must not exceed 255 characters");

        RuleFor(x => x.Message.Output2)
            .NotEmpty()
            .WithMessage("Output2 is required")
            .MaximumLength(255)
            .WithMessage("Output2 must not exceed 255 characters");

        RuleFor(x => x.Topic)
            .MaximumLength(256)
            .WithMessage("Topic must not exceed 256 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Topic));
    }
}
