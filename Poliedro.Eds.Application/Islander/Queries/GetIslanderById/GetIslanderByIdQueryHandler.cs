using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using System.Net;

namespace Poliedro.Eds.Application.Islander.Queries.GetIslanderById
{
    public class GetIslanderByIdQueryHandler(
        IIslanderGetByIdIslander islanderDomainIslander,
        IMapper mapper,
        IValidator<GetIslanderByIdQuery> validator)
        : IRequestHandler<GetIslanderByIdQuery, Result<IslanderDto, Error>>
    {
        public async Task<Result<IslanderDto, Error>> Handle(GetIslanderByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<IslanderDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await islanderDomainIslander.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<IslanderDto>(result.Value);
        }
    }
}
