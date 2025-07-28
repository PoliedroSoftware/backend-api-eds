using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory;
using Poliedro.Eds.Domain.HoseHistory.Entities;
using System.Net;

namespace Poliedro.Eds.Application.HoseHistory.Commands.CreateHoseHistory
{
    public class CreateHoseHistoryCommandHandler(
        IHoseHistoryCreateHoseHistory hosehistoryDomainHoseHistory,
        IMapper mapper,
        IValidator<CreateHoseHistoryRequestDto> validator
        ) : IRequestHandler<CreateHoseHistoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateHoseHistoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var hosehistoryEntity = mapper.Map<HoseHistoryEntity>(request.Request);
            var result = await hosehistoryDomainHoseHistory.CreateAsync(hosehistoryEntity);
            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}



