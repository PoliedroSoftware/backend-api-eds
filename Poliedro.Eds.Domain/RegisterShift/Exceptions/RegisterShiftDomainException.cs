using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Domain.RegisterShift.Exceptions;

public class RegisterShiftDomainException : Exception
{
    public RegisterShiftDomainException(string message) : base(message) { }
}
