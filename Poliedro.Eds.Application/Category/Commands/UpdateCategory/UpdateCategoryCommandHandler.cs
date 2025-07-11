using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler(
        ICategoryUpdateCategory categoryDomainCategory,
        IMapper mapper
   ) : IRequestHandler<UpdateCategoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryEntity = mapper.Map<CategoryEntity>(request);
            var result = await categoryDomainCategory.UpdateAsync(categoryEntity);

            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}
