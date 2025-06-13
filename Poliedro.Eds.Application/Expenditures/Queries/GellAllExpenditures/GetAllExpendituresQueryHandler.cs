using System.Diagnostics.Metrics;
using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Expenditures.Dtos;
using Poliedro.Eds.Domain.Expenditures.DomainExpenditures;

namespace Poliedro.Eds.Application.Expenditures.Queries.GellAllExpenditures;
public class GetAllExpendituresQueryHandler
(
    IExpendituresGetAllExpenditures ExpendituresDomainExpenditures,
    IMapper mapper)
    : IRequestHandler<GellAllExpendituresQuery, IEnumerable<ExpendituresDto>>
{
    public async Task<IEnumerable<ExpendituresDto>> Handle(GellAllExpendituresQuery request, CancellationToken cancellationToken)
    {
        var result = await ExpendituresDomainExpenditures.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<ExpendituresDto>>(result);
    }
}


