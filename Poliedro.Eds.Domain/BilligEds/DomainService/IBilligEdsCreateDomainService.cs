using System;
using System.Collections.Generic;
using System.Text;
using Poliedro.Eds.Domain.BilligEds.Entities;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Domain.BilligEds.DomainService;

public interface IBilligEdsCreateDomainService
{
    Task<Result<VoidResult, Error>> CreateBilligAsync(BillidEdsRequestEntity billigEdsRequest);
}
