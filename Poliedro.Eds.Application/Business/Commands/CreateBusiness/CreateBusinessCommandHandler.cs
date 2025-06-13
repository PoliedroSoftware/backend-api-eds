using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;

namespace Poliedro.Eds.Application.Business.Commands.CreateBusiness;

public class CreateBusinessCommandHandler(
    IBusinessCreateService BusinessCreateService,
    IMapper mapper) : IRequestHandler<CreateBusinessCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateBusinessCommand request, CancellationToken cancellationToken)
    {
        var BusinessEntity = mapper.Map<BusinessEntity>(request.Request);
        var result = await BusinessCreateService.CreateAsync(BusinessEntity);
        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!;
    }
}