using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Business.DomaianServices.Builder;
using Poliedro.Eds.Domain.Business.DomaianServices.Create;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Business.Commands.CreateBusiness;

public class CreateBusinessCommandHandler(
    IBusinessCreateDomianService businessCreateDomianService,
    IMapper mapper) : IRequestHandler<CreateBusinessCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateBusinessCommand request, CancellationToken cancellationToken)
    {
        BusinessEntity BusinessEntity = mapper.Map<BusinessEntity>(request.Request);
        BusinessEntity business = new BusinessBuilder()
                             .WithName(BusinessEntity.Name)
                             .WithContext(BusinessEntity.Context)
                             .Build();

        var result = await businessCreateDomianService.CreateAsync(business);
        return result.IsSuccess ? result.Value! : result.Error!;
    }
}