using FluentValidation;

namespace Poliedro.Eds.Application.OpenAI.Queries.GetChatHistory;

public class GetChatHistoryQueryValidator : AbstractValidator<GetChatHistoryQuery>
{
    public GetChatHistoryQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than zero");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than zero")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100");
    }
}