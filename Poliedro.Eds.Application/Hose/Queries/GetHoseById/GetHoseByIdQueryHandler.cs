using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Dtos;

namespace Poliedro.Eds.Application.Hose.Queries.GetHoseById
{
    public class GetHoseByIdQueryHandler(
        IHoseGetByIdHose hoseDomainHose,
        IMapper mapper)
        : IRequestHandler<GetHoseByIdQuery, Result<HoseDto, Error>>
    {
        public async Task<Result<HoseDto, Error>> Handle(GetHoseByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await hoseDomainHose.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<HoseDto>(result.Value);
        }
    }
}
