namespace Poliedro.Eds.Domain.Court.DomainService;

public interface IGetExpenditureName
{
    Task<string> GetExpenditureIdAsync(int id);
}
