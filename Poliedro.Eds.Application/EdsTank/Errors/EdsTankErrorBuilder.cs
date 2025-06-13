using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.EdsTank.Errors;

public class EdsTankErrorBuilder : IError
{
    public const string EDSTANK_CREATION_ERROR = "EdsTankCreationErrorException";
    public const string EDSTANK_NOT_FOUND_ERROR = "EdsTankNotFoundErrorException";
    public static Error EdsTankCreationException() => Error.CreateInstance(
        EDSTANK_CREATION_ERROR,
        "Failed to create Server due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string EDSTANK_UPDATE_ERROR = "EdsTankUpdateErrorException";

    public static Error EdsTankUpdateException() => Error.CreateInstance(
            EDSTANK_UPDATE_ERROR,
            "Failed to update EdsTank due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error EdsTankNotFoundException(int id) => Error.CreateInstance(
       EDSTANK_NOT_FOUND_ERROR,
       $"EdsTank with ID {id} was not found.",
       HttpStatusCode.NotFound);

}

