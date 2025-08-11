using Poliedro.Eds.Domain.Business.DomaianServices.Builder;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Business.Events;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Domain.Business.DomaianServices.Create;

public class BusinessDomainService(IBusinessCreateRepository repository) : IBusinessCreateDomianService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(BusinessEntity business)
    { 
      
    BusinessEntity BussinesCreated = new BusinessBuilder()
                            .WithName(business.Name)
                            .WithContext(business.Context)
                            .Build();

        return await repository.CreateAsync(BussinesCreated);
    }
}
