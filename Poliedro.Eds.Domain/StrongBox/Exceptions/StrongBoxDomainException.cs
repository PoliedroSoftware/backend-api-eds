using System;

namespace Poliedro.Eds.Domain.StrongBox.Exceptions
{
    public class StrongBoxDomainException : Exception
    {
        public StrongBoxDomainException(string message) : base(message) { }
    }
}
