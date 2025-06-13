using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Business.Commands.UpdateBusiness;

public class UpdateBusinessCommandHandler(IBusinessUpdateService BusinessUpdateService, IMapper mapper) : IRequestHandler<UpdateBusinessCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(UpdateBusinessCommand request,
        CancellationToken cancellationToken)
    {
        var BusinessEntity = mapper.Map<BusinessEntity>(request);
        var result = await BusinessUpdateService.UpdateAsync(BusinessEntity);

        if (!result.IsSuccess)
            return result.Error!;
        return result.Value!;
    }
}