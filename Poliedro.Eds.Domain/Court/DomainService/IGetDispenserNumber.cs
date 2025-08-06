namespace Poliedro.Eds.Domain.Court.DomainService;

public interface IGetDispenserNumber
{
    Task<int> GetDispenserNumberAsync(int id);
}
