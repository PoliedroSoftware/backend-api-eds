using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Court.Errors;

public class CourtErrorBuilder : IError
{
    public const string COURT_CREATION_ERROR = "CourtCreationErrorException";
    public const string COURT_NOT_FOUND_ERROR = " CourtNotFoundErrorException";
    public static Error CourtCreationException() => Error.CreateInstance(
        COURT_CREATION_ERROR,
        "Failed to create court due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string COURT_UPDATE_ERROR = "CourtUpdateErrorException";

    public static Error CourtUpdateException() => Error.CreateInstance(
            COURT_UPDATE_ERROR,
            "Failed to update Court due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error CourtNotFoundException(int id) => Error.CreateInstance(
       COURT_NOT_FOUND_ERROR,
       $"Court with ID {id} was not found.",
       HttpStatusCode.NotFound);

}

