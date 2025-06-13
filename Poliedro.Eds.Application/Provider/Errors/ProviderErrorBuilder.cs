using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Provider.Errors;

public class ProviderErrorBuilder : IError
{
    public const string Provider_CREATION_ERROR = "ProviderCreationErrorException";
    public const string Provider_NOT_FOUND_ERROR = "ProviderNotFoundErrorException";
    public static Error ProviderCreationException() => Error.CreateInstance(
       Provider_CREATION_ERROR,
        "Failed to create Provider due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string Provider_UPDATE_ERROR = "ProviderUpdateErrorException";

    public static Error ProviderUpdateException() => Error.CreateInstance(
            Provider_UPDATE_ERROR,
            "Failed to update Provider due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error ProviderNotFoundException(int id) => Error.CreateInstance(
      Provider_NOT_FOUND_ERROR,
       $"Provider with ID {id} was not found.",
       HttpStatusCode.NotFound);

}