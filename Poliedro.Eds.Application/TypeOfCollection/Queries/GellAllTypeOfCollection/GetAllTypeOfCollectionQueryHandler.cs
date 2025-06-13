using System.Diagnostics.Metrics;
using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.TypeOfCollection.Dtos;
using Poliedro.Eds.Domain.TypeOfCollection.DomainTypeOfCollection;

namespace Poliedro.Eds.Application.TypeOfCollection.Queries.GellAllTypeOfCollection;
public class GetAllTypeOfCollectionQueryHandler
(
    ITypeOfCollectionGetAllTypeOfCollection TypeOfCollectionDomainTypeOfCollection,
    IMapper mapper)
    : IRequestHandler<GellAllTypeOfCollectionQuery, IEnumerable<TypeOfCollectionDto>>
{
    public async Task<IEnumerable<TypeOfCollectionDto>> Handle(GellAllTypeOfCollectionQuery request, CancellationToken cancellationToken)
    {
        var result = await TypeOfCollectionDomainTypeOfCollection.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<TypeOfCollectionDto>>(result);
    }
}