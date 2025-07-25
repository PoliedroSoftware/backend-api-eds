using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler(
        ICategoryUpdateService categoryDomainCategory,
        IMapper mapper,
        IValidator<UpdateCategoryCommand> validator
        ) : IRequestHandler<UpdateCategoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var categoryEntity = mapper.Map<CategoryEntity>(request);
            var result = await categoryDomainCategory.UpdateAsync(categoryEntity);

            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}
