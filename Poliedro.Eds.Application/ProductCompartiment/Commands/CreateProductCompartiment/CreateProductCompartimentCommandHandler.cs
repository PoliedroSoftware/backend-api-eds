using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;
using Poliedro.Eds.Domain.ProductCompartiment.Entities;
using System.Net;

namespace Poliedro.Eds.Application.ProductCompartiment.Commands.CreateProductCompartiment;
    public class CreateProductCompartimentCommandHandler(
        IProductCompartimentCreateProductCompartiment ProductCompartimentDomainProductCompartiment,
        IMapper mapper,
        IValidator<CreateProductCompartimentRequestDto> validator,
        IRedisService redisService
        ) : IRequestHandler<CreateProductCompartimentCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateProductCompartimentCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var result = await ProductCompartimentDomainProductCompartiment.CreateAsync(mapper.Map<ProductCompartimentEntity>(request.Request));
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.PRODUCT_COMPARTMENT);
        return result.IsSuccess ? result.Value! : result.Error!;
    }
    }