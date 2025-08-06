namespace Poliedro.Eds.Domain.Court.DomainService;

public interface IGetHoseNumber
{
    Task<int> GetHoseNumberAsync(int id);
}
