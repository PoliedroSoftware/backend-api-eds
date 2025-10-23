using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.RegisterShift.Errors;

public class RegisterShiftBuilder : IError
{
    public const string REGISTER_SHIFT_CREATION_ERROR = "RegisterShiftCreationErrorException";
    public const string EDS_NOT_FOUND_ERROR = "EdsNotFoundErrorException";
    public static Error RegisterShiftCreationException() => Error.CreateInstance(
       REGISTER_SHIFT_CREATION_ERROR,
        "Failed to create RegisterShift due to an internal error.",
        HttpStatusCode.InternalServerError);

    public static Error EdsNotFoundException(int id) => Error.CreateInstance(
        EDS_NOT_FOUND_ERROR,
        $"Eds with ID {id} was not found.",
        HttpStatusCode.NotFound);

}
