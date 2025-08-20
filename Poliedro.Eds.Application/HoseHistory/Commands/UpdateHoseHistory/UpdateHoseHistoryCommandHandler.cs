using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory;
using Poliedro.Eds.Domain.HoseHistory.Entities;

namespace Poliedro.Eds.Application.HoseHistory.Commands.UpdateHoseHistory
{
    public class UpdateHoseHistoryCommandHandler(
        IHoseHistoryUpdateHoseHistory hosehistoryDomainHoseHistory,
        IMapper mapper,
        IValidator<UpdateHoseHistoryCommand> validator
        ) : IRequestHandler<UpdateHoseHistoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateHoseHistoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var hosehistoryEntity = mapper.Map<HoseHistoryEntity>(request);
            var result = await hosehistoryDomainHoseHistory.UpdateAsync(hosehistoryEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }
}
