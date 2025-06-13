using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Islander.Errors;

public class IslanderErrorBuilder : IError
{
    public const string ISLANDER_CREATION_ERROR = "IslanderCreationErrorException";
    public const string ISLANDER_NOT_FOUND_ERROR = "IslanderNotFoundErrorException";
    public static Error IslanderCreationException() => Error.CreateInstance(
       ISLANDER_CREATION_ERROR,
        "Failed to create Islander due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string ISLANDER_UPDATE_ERROR = "IslanderUpdateErrorException";

    public static Error IslanderUpdateException() => Error.CreateInstance(
            ISLANDER_UPDATE_ERROR,
            "Failed to update Islander due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error IslanderNotFoundException(int id) => Error.CreateInstance(
      ISLANDER_NOT_FOUND_ERROR,
       $"Islander with ID {id} was not found.",
       HttpStatusCode.NotFound);

}
