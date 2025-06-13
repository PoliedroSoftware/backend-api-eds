using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Expenditures.Entities;

namespace Poliedro.Eds.Domain.Expenditures.DomainExpenditures;

    public interface IExpendituresUpdateExpenditures
    {
        Task<Result<VoidResult, Error>> UpdateAsync(ExpendituresEntity ServerEntity); 
    }
