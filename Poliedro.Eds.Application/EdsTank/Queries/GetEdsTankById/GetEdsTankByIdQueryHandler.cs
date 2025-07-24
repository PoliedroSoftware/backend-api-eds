using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.EdsTank.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.EdsTank.DomainEdsTank;
using System.Net;

namespace Poliedro.Eds.Application.EdsTank.Queries.GetEdsTankById;
    public class GetEdsTankByIdQueryHandler(
        IEdsTankGetByIdEdsTank EdsTankDomainEdsTank,
        IMapper mapper,
        IValidator<GetEdsTankByIdQuery> validator)
        : IRequestHandler<GetEdsTankByIdQuery, Result<EdsTankDto, Error>>
    {
        public async Task<Result<EdsTankDto, Error>> Handle(GetEdsTankByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<EdsTankDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await EdsTankDomainEdsTank.GetByIdAsync(request.Id);
                if (!result.IsSuccess)
                    return result.Error!;

                return mapper.Map<EdsTankDto>(result.Value);
        }
    }