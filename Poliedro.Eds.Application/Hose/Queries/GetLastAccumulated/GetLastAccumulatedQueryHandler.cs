using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Hose.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;

namespace Poliedro.Eds.Application.Hose.Queries.GetLastAccumulated
{
    public class GetLastAccumulatedQueryHandler(
                    ILastAccumulatedService LastAccumulatedDomainService,
                    IMapper mapper)
                    : IRequestHandler<GetLastAccumulatedQuery, Result<LastAccumulatedDto, Error>>
    {
        public async Task<Result<LastAccumulatedDto, Error>> Handle(GetLastAccumulatedQuery request, CancellationToken cancellationToken)
        {
            var result = await LastAccumulatedDomainService.GetLastAccumulatedAsync(request.idDispenser, request.idHose);
            if (!result.IsSuccess)
                return Result<LastAccumulatedDto, Error>.Failure(result.Error!);

            var dto = mapper.Map<LastAccumulatedDto>(result.Value);
            return Result<LastAccumulatedDto, Error>.Success(dto);
        }
    }
}
