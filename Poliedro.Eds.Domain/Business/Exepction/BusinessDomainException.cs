namespace Poliedro.Eds.Domain.Business.Exepction;

public class BusinessDomainException : Exception
{
    public BusinessDomainException(string message) : base(message) { }
}
