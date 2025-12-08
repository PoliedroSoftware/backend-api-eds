using FluentValidation;
using Poliedro.Eds.Domain.BilligEds.Entities;

namespace Poliedro.Eds.Application.BilligEds.Commands.CreateBilligEds;

public class CreateBilligEdsCommandValidator : AbstractValidator<BillidEdsRequestEntity>
{
    public CreateBilligEdsCommandValidator()
    {
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Number).NotEmpty();
        RuleFor(x => x.TotalToPay).GreaterThanOrEqualTo(0);
    }
}
