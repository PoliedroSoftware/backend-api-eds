using System;
using System.Collections.Generic;
using System.Text;
using Poliedro.Eds.Domain.BilligEds.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Domain.BilligEds.DomainService;

public class BilligEdsCreateService : IBilligEdsCreateDomainService
{
    public Task<Result<VoidResult, Error>> CreateBilligAsync(BillidEdsRequestEntity billigEdsRequest)
    {
        throw new NotImplementedException();
    }
}
