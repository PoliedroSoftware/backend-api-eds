using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Category.Commands.CreateCategory;

public class CreateCategoryCommandHandler(
    ICategoryCreateService categoryDomainService,
    IMapper mapper,
    IValidator<CreateCategoryRequestDto> validator
    ) : IRequestHandler<CreateCategoryCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var categoryEntity = mapper.Map<CategoryEntity>(request.Request);
        var result = await categoryDomainService.CreateAsync(categoryEntity);
        if (!result.IsSuccess)
            return result.Error!;
        return result.Value!;
    }
}





