using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Dtos;
using System.Net;

namespace Poliedro.Eds.Application.Hose.Queries.GetHoseById
{
    public class GetHoseByIdQueryHandler(
        IHoseGetByIdHose hoseDomainHose,
        IMapper mapper,
        IValidator<GetHoseByIdQuery> validator)
        : IRequestHandler<GetHoseByIdQuery, Result<HoseDto, Error>>
    {
        public async Task<Result<HoseDto, Error>> Handle(GetHoseByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<HoseDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await hoseDomainHose.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<HoseDto>(result.Value);
        }
    }
}
