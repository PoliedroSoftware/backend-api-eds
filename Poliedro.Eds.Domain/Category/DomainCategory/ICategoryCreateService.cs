
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;


namespace Poliedro.Eds.Domain.Category.DomainCategory;

public interface ICategoryCreateService
{
    Task<Result<VoidResult, Error>> CreateAsync(CategoryEntity categoryEntity);
}

