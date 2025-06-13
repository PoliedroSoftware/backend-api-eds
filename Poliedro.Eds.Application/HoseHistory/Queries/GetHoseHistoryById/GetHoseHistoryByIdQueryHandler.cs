using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.HoseHistory.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory;

namespace Poliedro.Eds.Application.HoseHistory.Queries.GetHoseHistoryById
{
    public class GetHoseHistoryByIdQueryHandler(
        IHoseHistoryGetByIdHoseHistory hosehistoryDomainHoseHistory,
        IMapper mapper)
        : IRequestHandler<GetHoseHistoryByIdQuery, Result<HoseHistoryDto, Error>>
    {
        public async Task<Result<HoseHistoryDto, Error>> Handle(GetHoseHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await hosehistoryDomainHoseHistory.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<HoseHistoryDto>(result.Value);
        }
    }
}
