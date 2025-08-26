using System.Net;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Hose.Errors;

public class HoseErrorBuilder : IError
{
    public const string HOSE_CREATION_ERROR = "HoseCreationErrorException";
    public const string HOSE_NOT_FOUND_ERROR = "HoseNotFoundErrorException";
    public const string HOSE_LIMIT_REACHED_ERROR = "HoseLimitErrorException";
    public const string HOSE_LIST_EMPTY_ERROR = "HoseListEmptyException";

    public static Error HoseCreationException() => Error.CreateInstance(
       HOSE_CREATION_ERROR,
        "Failed to create Hose due to an internal error.",
        HttpStatusCode.InternalServerError);

    public static Error HoseLimitErrorException() => Error.CreateInstance(
      HOSE_CREATION_ERROR,
       "Maximum number of hoses reached.",
       HttpStatusCode.BadRequest);

    public const string HOSE_UPDATE_ERROR = "HoseUpdateErrorException";

    public static Error HoseUpdateException() => Error.CreateInstance(
       HOSE_UPDATE_ERROR,
       "Failed to update Hose due to an internal error.",
       HttpStatusCode.InternalServerError);

    public static Error HoseNotFoundException(int id) => id == 0 
        ? Error.CreateInstance(
            HOSE_LIST_EMPTY_ERROR,
            "No hoses found in the system.",
            HttpStatusCode.NotFound)
        : Error.CreateInstance(
            HOSE_NOT_FOUND_ERROR,
            $"Hose with ID {id} was not found.",
            HttpStatusCode.NotFound);

}
