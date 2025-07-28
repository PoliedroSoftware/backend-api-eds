

namespace Poliedro.Eds.Domain.Hose.DomainHose
{
    public interface IHoseQueryService
    {
        Task<int?> GetHoseLimitAsync(int dispenserId);
        Task<int> GetCurrentHoseCountAsync(int dispenserId);
    }
}
