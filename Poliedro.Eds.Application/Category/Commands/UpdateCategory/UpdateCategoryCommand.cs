using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Category.Commands.UpdateCategory;

    public record UpdateCategoryCommand(
    int IdCategory,
    string Description) : IRequest<Result<VoidResult, Error>>;
