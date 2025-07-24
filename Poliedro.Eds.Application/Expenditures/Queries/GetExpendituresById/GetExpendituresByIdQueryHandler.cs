using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Expenditures.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Expenditures.DomainExpenditures;
using System.Net;

namespace Poliedro.Eds.Application.Expenditures.Queries.GetExpendituresById;

    public class GetExpendituresByIdQueryHandler(
        IExpendituresGetByIdExpenditures ExpendituresDomainExpenditures,
        IMapper mapper,
        IValidator<GetExpendituresByIdQuery> validator)
        : IRequestHandler<GetExpendituresByIdQuery, Result<ExpendituresDto, Error>>
    {
        public async Task<Result<ExpendituresDto, Error>> Handle(GetExpendituresByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<ExpendituresDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await ExpendituresDomainExpenditures.GetByIdAsync(request.Id);
                if (!result.IsSuccess)
                    return result.Error!;

                return mapper.Map<ExpendituresDto>(result.Value);
        }
    }