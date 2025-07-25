using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Expenditures.DomainExpenditures;
using Poliedro.Eds.Domain.Expenditures.Entities;
using System.Net;

namespace Poliedro.Eds.Application.Expenditures.Commands.CreateExpenditures;
    public class CreateExpendituresCommandHandler(
        IExpendituresCreateExpenditures ExpendituresDomainExpenditures,
        IMapper mapper,
        IValidator<CreateExpendituresRequestDto> validator
        ) : IRequestHandler<CreateExpendituresCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateExpendituresCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

                var ExpendituresEntity = mapper.Map<ExpendituresEntity>(request.Request);
                var result = await ExpendituresDomainExpenditures.CreateAsync(ExpendituresEntity);
                if (!result.IsSuccess)
                    return result.Error!;

                return result.Value!;
        }
    }





