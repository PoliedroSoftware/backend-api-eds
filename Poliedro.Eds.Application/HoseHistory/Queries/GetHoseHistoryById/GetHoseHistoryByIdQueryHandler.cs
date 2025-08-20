using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.HoseHistory.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory;

namespace Poliedro.Eds.Application.HoseHistory.Queries.GetHoseHistoryById
{
    public class GetHoseHistoryByIdQueryHandler(
        IHoseHistoryGetByIdHoseHistory hosehistoryDomainHoseHistory,
        IMapper mapper,
        IValidator<GetHoseHistoryByIdQuery> validator)
        : IRequestHandler<GetHoseHistoryByIdQuery, Result<HoseHistoryDto, Error>>
    {
        public async Task<Result<HoseHistoryDto, Error>> Handle(GetHoseHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<HoseHistoryDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await hosehistoryDomainHoseHistory.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<HoseHistoryDto>(result.Value);
        }
    }
}
