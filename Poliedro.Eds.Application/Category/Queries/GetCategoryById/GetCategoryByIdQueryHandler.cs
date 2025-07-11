using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Category.Dtos;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Category.Queries.GetCategoryById;
    public class GetCategoryByIdQueryHandler(
        ICategoryGetByIdCategory categoryDomainService,
        IMapper mapper)
        : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto, Error>>
    {
        public async Task<Result<CategoryDto, Error>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await categoryDomainService.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<CategoryDto>(result.Value);
        }
    }

