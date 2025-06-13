using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory;
using Poliedro.Eds.Domain.HoseHistory.Entities;

namespace Poliedro.Eds.Application.HoseHistory.Commands.CreateHoseHistory
{
    public class CreateHoseHistoryCommandHandler(
        IHoseHistoryCreateHoseHistory hosehistoryDomainHoseHistory,
        IMapper mapper) : IRequestHandler<CreateHoseHistoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateHoseHistoryCommand request, CancellationToken cancellationToken)
        {      

            var hosehistoryEntity = mapper.Map<HoseHistoryEntity>(request.Request);
            var result = await hosehistoryDomainHoseHistory.CreateAsync(hosehistoryEntity);
            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;    
        }
    }
}



