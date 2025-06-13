using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Dispensers.Errors;

public class DispensersErrorBuilder : IError
{
    public const string DISPENSERS_CREATION_ERROR = "DispensersCreationErrorException";
    public const string DISPENSERS_NOT_FOUND_ERROR = "DispensersNotFoundErrorException";
    public static Error DispensersCreationException() => Error.CreateInstance(
        DISPENSERS_CREATION_ERROR,
        "Failed to create Server due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string DISPENSERS_UPDATE_ERROR = "DispensersUpdateErrorException";

    public static Error DispensersUpdateException() => Error.CreateInstance(
            DISPENSERS_UPDATE_ERROR,
            "Failed to update Server due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error DispensersNotFoundException(int id) => Error.CreateInstance(
       DISPENSERS_NOT_FOUND_ERROR,
       $"Dispensers with ID {id} was not found.",
       HttpStatusCode.NotFound);

}

