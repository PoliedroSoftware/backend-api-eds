using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Tank.Dtos;
using Poliedro.Eds.Domain.Tank.DomainTank;

namespace Poliedro.Eds.Application.Tank.Queries.GellAllTank;
public class GetAllTankQueryHandler
(
    ITankGetAllTank tankDomainTank,
    IMapper mapper)
    : IRequestHandler<GellAllTankQuery, IEnumerable<TankDto>>
{
    public async Task<IEnumerable<TankDto>> Handle(GellAllTankQuery request, CancellationToken cancellationToken)
    {
        var result = await tankDomainTank.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<TankDto>>(result);
    }
}

