using System.Diagnostics.Metrics;
using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.DashboardPowerBI.TypeOfCollectionView.DomainTypeOfCollectionView;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllTypeOfCollection;

public class GetAllTypeOfCollectionQueryHandler
(
    ITypeOfCollectionViewGetAllTypeOfCollection TypeOfCollectionViewDomainTypeOfCollection,
    IMapper mapper)
    : IRequestHandler<GellAllTypeOfCollectionQuery, IEnumerable<TypeOfCollectionDto>>
{
    public async Task<IEnumerable<TypeOfCollectionDto>> Handle(GellAllTypeOfCollectionQuery request, CancellationToken cancellationToken)
    {
        var result = await TypeOfCollectionViewDomainTypeOfCollection.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<TypeOfCollectionDto>>(result);
    }
}