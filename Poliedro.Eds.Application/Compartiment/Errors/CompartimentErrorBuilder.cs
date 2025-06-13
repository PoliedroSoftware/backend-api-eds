using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Compartiment.Errors;

public class CompartimentErrorBuilder : IError
{
    public const string COMPARTIMENT_CREATION_ERROR = "CompartimentCreationErrorException";
    public const string COMPARTIMENT_NOT_FOUND_ERROR = "CompartimentNotFoundErrorException";
    public static Error CompartimentCreationException() => Error.CreateInstance(
        COMPARTIMENT_CREATION_ERROR,
        "Failed to create Server due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string COMPARTIMENT_UPDATE_ERROR = "CompartimentUpdateErrorException";

    public static Error CompartimentUpdateException() => Error.CreateInstance(
            COMPARTIMENT_UPDATE_ERROR,
            "Failed to update Server due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error CompartimentNotFoundException(int id) => Error.CreateInstance(
       COMPARTIMENT_NOT_FOUND_ERROR,
       $"Compartiment with ID {id} was not found.",
       HttpStatusCode.NotFound);

}

