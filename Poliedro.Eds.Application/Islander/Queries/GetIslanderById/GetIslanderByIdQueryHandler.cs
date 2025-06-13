using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.DomainIslander;

namespace Poliedro.Eds.Application.Islander.Queries.GetIslanderById
{
    public class GetIslanderByIdQueryHandler(
        IIslanderGetByIdIslander islanderDomainIslander,
        IMapper mapper)
        : IRequestHandler<GetIslanderByIdQuery, Result<IslanderDto, Error>>
    {
        public async Task<Result<IslanderDto, Error>> Handle(GetIslanderByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await islanderDomainIslander.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<IslanderDto>(result.Value);
        }
    }
}
