using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Category.Commands.CreateCategory;
    public class CreateCategoryCommandHandler(
        ICategoryCreateCategory categoryDomainService,
        IMapper mapper) : IRequestHandler<CreateCategoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
        var categoryEntity = mapper.Map<CategoryEntity>(request.Request);
        var result = await categoryDomainService.CreateAsync(categoryEntity);
            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }





