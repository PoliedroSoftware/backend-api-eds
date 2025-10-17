namespace Poliedro.Eds.Domain.TransferValidation.Exceptions;

public class TransferValidationDomainException : Exception
{
    public TransferValidationDomainException(string message) : base(message)
    {
    }

    public TransferValidationDomainException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
