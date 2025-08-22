namespace Poliedro.Eds.Domain.Eds.DomainEds;

public interface IGetEdsName
{
    Task<string> GetEdsNameAsync(int idEds);
}
