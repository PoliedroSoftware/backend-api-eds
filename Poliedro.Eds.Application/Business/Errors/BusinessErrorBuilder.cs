using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Business.Errors;

public class BusinessErrorBuilder : IError
{
    public const string Business_CREATION_ERROR = "BusinessCreationErrorException";
    public const string Business_NOT_FOUND_ERROR = "BusinessNotFoundErrorException";
    public static Error BusinessCreationException() => Error.CreateInstance(
       Business_CREATION_ERROR,
        "Failed to create Business due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string Business_UPDATE_ERROR = "BusinessUpdateErrorException";

    public static Error BusinessUpdateException() => Error.CreateInstance(
            Business_UPDATE_ERROR,
            "Failed to update Business due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error BusinessNotFoundException(int id) => Error.CreateInstance(
      Business_NOT_FOUND_ERROR,
       $"Business with ID {id} was not found.",
       HttpStatusCode.NotFound);

}