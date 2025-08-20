using Poliedro.Eds.Domain.Business.DomaianServices.Builder;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Common.Events;

namespace Poliedro.Eds.Domain.Business.DomaianServices.Create;

public class BusinessDomainService(
    IBusinessCreateRepository _repository,
    IDomainEventDispatcher _domainEventDispatcher) : IBusinessCreateDomianService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(BusinessEntity business)
    {
        
        var businessCreated = new BusinessBuilder()
            .WithName(business.Name)
            .WithContext(business.Context)
            .Build();

        var result = await _repository.CreateAsync(businessCreated);
        
        if (!result.IsSuccess)
        {
            return result;
        }

        await _domainEventDispatcher.DispatchAsync(businessCreated.DomainEvents);
        
        businessCreated.ClearDomainEvents();

        return result;
    }
}
