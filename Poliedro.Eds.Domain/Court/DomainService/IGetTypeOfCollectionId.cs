namespace Poliedro.Eds.Domain.Court.DomainService;

public interface IGetTypeOfCollectionId
{
    Task<int?> GetTypeOfCollectionIdAsync(string typeOfCollectionName);
}
