using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory;
using Poliedro.Eds.Domain.HoseHistory.Entities;

namespace Poliedro.Eds.Application.HoseHistory.Commands.UpdateHoseHistory
{
    public class UpdateHoseHistoryCommandHandler(
        IHoseHistoryUpdateHoseHistory hosehistoryDomainHoseHistory,
        IMapper mapper
    ) : IRequestHandler<UpdateHoseHistoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateHoseHistoryCommand request, CancellationToken cancellationToken)
        {
            var hosehistoryEntity = mapper.Map<HoseHistoryEntity>(request);
            var result = await hosehistoryDomainHoseHistory.UpdateAsync(hosehistoryEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }
}