using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Shopping.Errors;

public class ShoppingErrorBuilder : IError
{
    public const string SHOPPING_CREATION_ERROR = "ShoppingCreationErrorException";
    public const string SHOPPING_NOT_FOUND_ERROR = "ShoppingNotFoundErrorException";
    public static Error ShoppingCreationException() => Error.CreateInstance(
        SHOPPING_CREATION_ERROR,
        "Failed to create Server due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string SHOPPING_UPDATE_ERROR = "ShoppingUpdateErrorException";

    public static Error ShoppingUpdateException() => Error.CreateInstance(
            SHOPPING_UPDATE_ERROR,
            "Failed to update Server due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error ShoppingNotFoundException(int id) => Error.CreateInstance(
       SHOPPING_NOT_FOUND_ERROR,
       $"Shopping with ID {id} was not found.",
       HttpStatusCode.NotFound);

}

