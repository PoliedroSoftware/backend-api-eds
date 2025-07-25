using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Expenditures.DomainExpenditures;
using Poliedro.Eds.Domain.Expenditures.Entities;
using System.Net;

namespace Poliedro.Eds.Application.Expenditures.Commands.UpdateExpenditures;

    public class UpdateExpendituresCommandHandler(
        IExpendituresUpdateExpenditures ExpendituresDomainExpenditures,
        IMapper mapper,
        IValidator<UpdateExpendituresCommand> validator
        ) : IRequestHandler<UpdateExpendituresCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateExpendituresCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var ExpendituresEntity = mapper.Map<ExpendituresEntity>(request);
                var result = await ExpendituresDomainExpenditures.UpdateAsync(ExpendituresEntity);

                if (!result.IsSuccess)
                    return result.Error!;

                return result.Value!;
        }
    }