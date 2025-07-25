using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Business.Queries.GetBusinessById;
using Poliedro.Eds.Application.Category.Dtos;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Category.Queries.GetCategoryById;
    public class GetCategoryByIdQueryHandler(
        ICategoryGetByIdService categoryDomainService,
        IMapper mapper,
        IValidator<GetCategoryByIdQuery> validator)
        : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto, Error>>
    {
        public async Task<Result<CategoryDto, Error>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
        var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<CategoryDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }

            var result = await categoryDomainService.GetByIdAsync(request.Id);
                if (!result.IsSuccess)
                    return result.Error!;

                return mapper.Map<CategoryDto>(result.Value);
            }
    }

