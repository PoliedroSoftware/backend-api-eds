using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Islander.Commands.CreateIslander;

public class CreateIslanderCommandValidator : AbstractValidator<CreateIslanderRequestDto>
{
    public CreateIslanderCommandValidator(IRedisService redisService)
    {
        var messages = LoadMessages(redisService).GetAwaiter().GetResult();

        RuleFor(x => x.Name)
            .NotNull().WithMessage(messages["NameNotNull"])
            .NotEmpty().WithMessage(messages["NameNotEmpty"]);

        RuleFor(x => x.IdEds)
            .NotNull().WithMessage(messages["IdEdsNotNull"])
            .GreaterThan(0).WithMessage(messages["IdEdsNotEmpty"]);

        RuleFor(x => x.Password)
            .NotNull().WithMessage(messages["PasswordNotNull"])
            .NotEmpty().WithMessage(messages["PasswordNotEmpty"]);

        RuleFor(x => x.Email)
            .NotNull().WithMessage(messages["EmailNotNull"])
            .NotEmpty().WithMessage(messages["EmailNotEmpty"])
            .EmailAddress().WithMessage("EmailInvalid");

        RuleFor(x => x.FirstName)
            .NotNull().WithMessage(messages["FirstNameNotNull"])
            .NotEmpty().WithMessage(messages["FirstNameNotEmpty"]);

        RuleFor(x => x.LastName)
            .NotNull().WithMessage(messages["LastNameNotNull"])
            .NotEmpty().WithMessage(messages["LastNameNotEmpty"]);

    }

    private async Task<Dictionary<string, string>> LoadMessages(IRedisService redis)
    {
        var keys = new[]
        {
            "NameNotNull", "NameNotEmpty",
            "IdEdsNotNull", "IdEdsNotEmpty",
            "PasswordNotNull", "PasswordNotEmpty",
            "EmailNotNull", "EmailNotEmpty", "EmailInvalid",
            "FirstNameNotNull", "FirstNameNotEmpty",
            "LastNameNotNull", "LastNameNotEmpty"
        };

        var result = new Dictionary<string, string>();
        foreach (var key in keys)
        {
            result[key] = await redis.GetValueFromCacheAsync(key) ?? $"Mensaje {key} perdido!!";
        }
        return result;
    }
}
