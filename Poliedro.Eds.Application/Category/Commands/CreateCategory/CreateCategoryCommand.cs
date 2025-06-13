using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Category.Commands.CreateCategory;

public record CreateCategoryCommand(CreateCategoryRequestDto Request) : IRequest<Result<VoidResult, Error>>;