using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Expenditures.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Expenditures.DomainExpenditures;

namespace Poliedro.Eds.Application.Expenditures.Queries.GetExpendituresById;

    public class GetExpendituresByIdQueryHandler(
        IExpendituresGetByIdExpenditures ExpendituresDomainExpenditures,
        IMapper mapper)
        : IRequestHandler<GetExpendituresByIdQuery, Result<ExpendituresDto, Error>>
    {
        public async Task<Result<ExpendituresDto, Error>> Handle(GetExpendituresByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await ExpendituresDomainExpenditures.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<ExpendituresDto>(result.Value);
        }
    }