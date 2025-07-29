using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Business.Queries.GetBusinessById;
using Poliedro.Eds.Application.DispenserType.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.DispenserType.DomainDispenserType;

namespace Poliedro.Eds.Application.DispenserType.Queries.GetDispenserTypeById
{
    public class GetDispenserTypeByIdQueryHandler(
        IDispenserTypeGetByIdDispenserType dispenserTypeDomainService,
        IMapper mapper,
        IValidator<GetDispenserTypeByIdQuery> validator)
        : IRequestHandler<GetDispenserTypeByIdQuery, Result<DispenserTypeDto, Error>>
    {
        public async Task<Result<DispenserTypeDto, Error>> Handle(GetDispenserTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<DispenserTypeDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await dispenserTypeDomainService.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<DispenserTypeDto>(result.Value);
        }
    }
}
