using FluentValidation;
using Poliedro.Eds.Application.OpenAI.Dtos;

namespace Poliedro.Eds.Application.OpenAI.Commands.SendMessage;

public class SendOpenAIMessageCommandValidator : AbstractValidator<OpenAIRequestDto>
{
    public SendOpenAIMessageCommandValidator()
    {
        RuleFor(x => x.UserMessage)
            .NotEmpty()
            .WithMessage("User message is required")
            .MaximumLength(4000)
            .WithMessage("User message cannot exceed 4000 characters");

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Model is required")
            .Must(model => IsValidModel(model))
            .WithMessage("Invalid OpenAI model specified");

        RuleFor(x => x.MaxTokens)
            .GreaterThan(0)
            .WithMessage("Max tokens must be greater than zero")
            .LessThanOrEqualTo(4000)
            .WithMessage("Max tokens cannot exceed 4000");

        RuleFor(x => x.Temperature)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Temperature must be at least 0")
            .LessThanOrEqualTo(2)
            .WithMessage("Temperature cannot exceed 2");

        RuleFor(x => x.SystemMessage)
            .MaximumLength(1000)
            .WithMessage("System message cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.SystemMessage));
    }

    private static bool IsValidModel(string model)
    {
        var validModels = new[]
        {
            "gpt-3.5-turbo",
            "gpt-3.5-turbo-16k",
            "gpt-4",
            "gpt-4-32k",
            "gpt-4-turbo-preview",
            "gpt-4o",
            "gpt-4o-mini"
        };
        
        return validModels.Contains(model);
    }
}