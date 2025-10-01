using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Phone.Entities;

namespace Poliedro.Eds.Domain.Phone.DomainServices.GetByNumber;

public interface IPhoneGetByNumberService
{
    Task<Result<PhoneEntity?, Error>> GetByNumberAsync(string number);
    Task<bool> ExistsAsync(string number);
}
