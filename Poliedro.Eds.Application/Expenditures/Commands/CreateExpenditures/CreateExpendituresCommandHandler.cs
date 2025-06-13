using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Expenditures.DomainExpenditures;
using Poliedro.Eds.Domain.Expenditures.Entities;

namespace Poliedro.Eds.Application.Expenditures.Commands.CreateExpenditures;
    public class CreateExpendituresCommandHandler(
        IExpendituresCreateExpenditures ExpendituresDomainExpenditures,
        IMapper mapper) : IRequestHandler<CreateExpendituresCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateExpendituresCommand request, CancellationToken cancellationToken)
        {
            var ExpendituresEntity = mapper.Map<ExpendituresEntity>(request.Request);
            var result = await ExpendituresDomainExpenditures.CreateAsync(ExpendituresEntity);
            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }





