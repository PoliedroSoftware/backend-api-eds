namespace Poliedro.Eds.Domain.Court.DomainService;

public interface IGetExpenditureId
{
    Task<int?> GetExpenditureIdAsync(string expenditureName);
}
