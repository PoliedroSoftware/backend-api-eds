using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Category.Dtos;
using Poliedro.Eds.Domain.Category.DomainCategory;

namespace Poliedro.Eds.Application.Category.Queries.GellAllCategory;
public class GetAllCategoryQueryHandler
(
    ICategoryGetAllCategory categoryDomainService,
    IMapper mapper)
    : IRequestHandler<GellAllCategoryQuery, IEnumerable<CategoryDto>>
{
    public async Task<IEnumerable<CategoryDto>> Handle(GellAllCategoryQuery request, CancellationToken cancellationToken)
    {
        var result = await categoryDomainService.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<CategoryDto>>(result);
    }
}


