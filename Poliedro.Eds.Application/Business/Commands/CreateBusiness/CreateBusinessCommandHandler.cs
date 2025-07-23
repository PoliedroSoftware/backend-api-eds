using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Business.Helpers;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Business.DomaianServices.Create;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Business.Commands.CreateBusiness;

public class CreateBusinessCommandHandler(
    IBusinessCreateDomianService businessCreateDomianService,
    IMapper mapper, 
    IRedisService redisService) : IRequestHandler<CreateBusinessCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateBusinessCommand request, CancellationToken cancellationToken)
    {
        var result = await businessCreateDomianService.CreateAsync(mapper.Map<BusinessEntity>(request.Request));
        await RedisHelper.RemoveBusinessCacheIfSuccessAsync(result, redisService);
        return result.IsSuccess ? result.Value! : result.Error!;
    }
}