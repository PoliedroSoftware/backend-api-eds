using MediatR;
using Poliedro.Eds.Application.Category.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Category.Queries.GetCategoryById;
public record GetCategoryByIdQuery(int Id) : IRequest<Result<CategoryDto, Error>>;
