using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Eds.Errors;

public class EdsErrorBuilder : IError
{
    public const string Eds_CREATION_ERROR = "EdsCreationErrorException";
    public const string Eds_NOT_FOUND_ERROR = "EdsNotFoundErrorException";
    public static Error EdsCreationException() => Error.CreateInstance(
       Eds_CREATION_ERROR,
        "Failed to create Eds due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string Eds_UPDATE_ERROR = "EdsUpdateErrorException";

    public static Error EdsUpdateException() => Error.CreateInstance(
            Eds_UPDATE_ERROR,
            "Failed to update Eds due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error EdsNotFoundException(int id) => Error.CreateInstance(
      Eds_NOT_FOUND_ERROR,
       $"Eds with ID {id} was not found.",
       HttpStatusCode.NotFound);

}