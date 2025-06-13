using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Hose.Errors;

public class HoseErrorBuilder : IError
{
    public const string HOSE_CREATION_ERROR = "HoseCreationErrorException";
    public const string HOSE_NOT_FOUND_ERROR = "HoseNotFoundErrorException";
    public static Error HoseCreationException() => Error.CreateInstance(
       HOSE_CREATION_ERROR,
        "Failed to create Hose due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string HOSE_UPDATE_ERROR = "HoseUpdateErrorException";

    public static Error HoseUpdateException() => Error.CreateInstance(
       HOSE_UPDATE_ERROR,
       "Failed to update Hose due to an internal error.",
       HttpStatusCode.InternalServerError);
    public static Error HoseNotFoundException(int id) => Error.CreateInstance(
      HOSE_NOT_FOUND_ERROR,
       $"Hose with ID {id} was not found.",
       HttpStatusCode.NotFound);

}
