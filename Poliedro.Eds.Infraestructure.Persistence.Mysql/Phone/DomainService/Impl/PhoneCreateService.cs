using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Phone.DomainServices.Create;
using Poliedro.Eds.Domain.Phone.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Phone.Errors;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Phone.DomainService.Impl;

public class PhoneCreateService(ITenantDbContextFactory dbContextFactory) : IPhoneCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(PhoneEntity phoneEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.Phone.AddAsync(phoneEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return PhoneErrorBuilder.PhoneCreationException();
        return VoidResult.Instance;
    }
}
