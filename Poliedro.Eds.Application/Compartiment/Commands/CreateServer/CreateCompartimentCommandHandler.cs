using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;

namespace Poliedro.Eds.Application.Compartiment.Commands.CreateCompartiment
{
    public class CreateCompartimentCommandHandler(
        ICompartimentCreateService compartimentDomainService,
        IMapper mapper,
        IValidator<CreateCompartimentRequestDto> validator,
        IRedisService redisService
        ) : IRequestHandler<CreateCompartimentCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateCompartimentCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var result = await compartimentDomainService.CreateAsync(mapper.Map<CompartimentEntity>(request.Request));
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.COMPARTIMENT);
<<<<<<< HEAD
            return result;
=======
            return result.IsSuccess ? result.Value! : result.Error!;
>>>>>>> New-service-StrongBox
        }
    }
}




