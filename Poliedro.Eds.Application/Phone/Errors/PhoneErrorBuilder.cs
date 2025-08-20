using System.Net;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Phone.Errors;

public class PhoneErrorBuilder : IError
{
    public const string PHONE_CREATION_ERROR = "PhoneCreationErrorException";
    public const string PHONE_NOT_FOUND_ERROR = "PhoneNotFoundErrorException";
    public const string PHONE_UPDATE_ERROR = "PhoneUpdateErrorException";

    public static Error PhoneCreationException() => Error.CreateInstance(
        PHONE_CREATION_ERROR,
        "Failed to create Phone due to an internal error.",
        HttpStatusCode.InternalServerError);

    public static Error PhoneUpdateException() => Error.CreateInstance(
        PHONE_UPDATE_ERROR,
        "Failed to update Phone due to an internal error.",
        HttpStatusCode.InternalServerError);

    public static Error PhoneNotFoundException(int id) => Error.CreateInstance(
        PHONE_NOT_FOUND_ERROR,
        $"Phone with ID {id} was not found.",
        HttpStatusCode.NotFound);
}
