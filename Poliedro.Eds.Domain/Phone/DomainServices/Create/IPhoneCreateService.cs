using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Phone.Entities;
using System.Threading.Tasks;

namespace Poliedro.Eds.Domain.Phone.DomainServices.Create;

public interface IPhoneCreateService
{
    Task<Result<VoidResult, Error>> CreateAsync(PhoneEntity phoneEntity);
}
