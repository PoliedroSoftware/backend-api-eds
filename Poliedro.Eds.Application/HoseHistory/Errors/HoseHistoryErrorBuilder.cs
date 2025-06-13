using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.HoseHistory.Errors;

public class HoseHistoryErrorBuilder : IError
{
    public const string HOSEHISTORY_CREATION_ERROR = "HoseHistoryCreationErrorException";
    public const string HOSEHISTORY_NOT_FOUND_ERROR = "HoseHistoryNotFoundErrorException";
    public static Error HoseHistoryCreationException() => Error.CreateInstance(
       HOSEHISTORY_CREATION_ERROR,
        "Failed to create HoseHistory due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string HOSEHISTORY_UPDATE_ERROR = "HoseHistoryUpdateErrorException";

    public static Error HoseHistoryUpdateException() => Error.CreateInstance(
       HOSEHISTORY_UPDATE_ERROR,
       "Failed to update HoseHistory due to an internal error.",
       HttpStatusCode.InternalServerError);
    public static Error HoseHistoryNotFoundException(int id) => Error.CreateInstance(
      HOSEHISTORY_NOT_FOUND_ERROR,
       $"HoseHistory with ID {id} was not found.",
       HttpStatusCode.NotFound);

}
