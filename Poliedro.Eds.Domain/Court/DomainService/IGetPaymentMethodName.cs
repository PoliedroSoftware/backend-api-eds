namespace Poliedro.Eds.Domain.Court.DomainService;

public interface IGetPaymentMethodName
{
    Task<string> GetPaymentMethodNameAsync(int id);
}
