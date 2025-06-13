using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.DispenserType.Errors;

public class DispenserTypeErrorBuilder : IError
{
    public const string DISPENSERTYPE_CREATION_ERROR = "DispenserTypeCreationErrorException";
    public const string DISPENSERTYPE_NOT_FOUND_ERROR = "DispenserTypeNotFoundErrorException";
    public static Error DispenserTypeCreationException() => Error.CreateInstance(
        DISPENSERTYPE_CREATION_ERROR,
        "Failed to create Server due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string DISPENSERTYPE_UPDATE_ERROR = "DispenserTypeUpdateErrorException";

    public static Error DispenserTypeUpdateException() => Error.CreateInstance(
            DISPENSERTYPE_UPDATE_ERROR,
            "Failed to update Server due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error DispenserTypeNotFoundException(int id) => Error.CreateInstance(
       DISPENSERTYPE_NOT_FOUND_ERROR,
       $"DispenserType with ID {id} was not found.",
       HttpStatusCode.NotFound);

}

