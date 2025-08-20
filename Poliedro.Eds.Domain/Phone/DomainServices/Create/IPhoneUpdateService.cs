using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Phone.Entities;
using System.Threading.Tasks;

namespace Poliedro.Eds.Domain.Phone.DomainServices.Update;

public interface IPhoneUpdateService
{
    Task<Result<VoidResult, Error>> UpdateAsync(PhoneEntity phoneEntity);
}
