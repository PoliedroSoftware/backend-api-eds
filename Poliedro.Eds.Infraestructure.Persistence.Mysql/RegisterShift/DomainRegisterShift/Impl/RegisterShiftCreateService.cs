using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Tls;
using Poliedro.Eds.Application.RegisterShift.Errors;
using Poliedro.Eds.Application.RegisterShift.Validations;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.RegisterShift.DomainService;
using Poliedro.Eds.Domain.RegisterShift.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.RegisterShift.DomainRegisterShift.Impl;

public class RegisterShiftCreateService(ITenantDbContextFactory dbContextFactory) : IRegisterShiftCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(RegisterShiftEntity entity, CancellationToken cancellationToken)
    {
        using var db = dbContextFactory.CreateDbContext();
        await db.AddAsync(entity, cancellationToken);
        var result = await db.SaveChangesAsync() >0;
        if (!result)
            return RegisterShiftBuilder.RegisterShiftCreationException();
        return VoidResult.Instance;
    }
}
