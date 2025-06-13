using MediatR;
using Poliedro.Eds.Application.Category.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.Category.Queries.GellAllCategory;
public record GellAllCategoryQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<CategoryDto>>;
