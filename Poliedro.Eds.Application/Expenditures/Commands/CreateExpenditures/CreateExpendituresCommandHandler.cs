using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Expenditures.DomainExpenditures;
using Poliedro.Eds.Domain.Expenditures.Entities;
using System.Net;

namespace Poliedro.Eds.Application.Expenditures.Commands.CreateExpenditures;
    public class CreateExpendituresCommandHandler(
        IExpendituresCreateExpenditures ExpendituresDomainExpenditures,
        IMapper mapper,
        IValidator<CreateExpendituresRequestDto> validator,
        IRedisService redisService
        ) : IRequestHandler<CreateExpendituresCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateExpendituresCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var result = await ExpendituresDomainExpenditures.CreateAsync(mapper.Map<ExpendituresEntity>(request.Request));
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.EXPENDITURES);
        return result.IsSuccess ? result.Value! : result.Error!;
    }
    }





