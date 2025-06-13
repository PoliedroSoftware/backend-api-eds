using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Expenditures.DomainExpenditures;
using Poliedro.Eds.Domain.Expenditures.Entities;

namespace Poliedro.Eds.Application.Expenditures.Commands.UpdateExpenditures;

    public class UpdateExpendituresCommandHandler(
        IExpendituresUpdateExpenditures ExpendituresDomainExpenditures,
        IMapper mapper
    ) : IRequestHandler<UpdateExpendituresCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateExpendituresCommand request, CancellationToken cancellationToken)
        {
            var ExpendituresEntity = mapper.Map<ExpendituresEntity>(request);
            var result = await ExpendituresDomainExpenditures.UpdateAsync(ExpendituresEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }