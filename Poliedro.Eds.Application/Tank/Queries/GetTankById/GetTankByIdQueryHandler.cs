using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Tank.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Tank.DomainTank;

namespace Poliedro.Eds.Application.Tank.Queries.GetTankById
{
    public class GetTankByIdQueryHandler(
        ITankGetByIdTank tankDomainTank,
        IMapper mapper,
        IValidator<GetTankByIdQuery> validator)
        : IRequestHandler<GetTankByIdQuery, Result<TankDto, Error>>
    {
        public async Task<Result<TankDto, Error>> Handle(GetTankByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<TankDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await tankDomainTank.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<TankDto>(result.Value);
        }
    }
}
