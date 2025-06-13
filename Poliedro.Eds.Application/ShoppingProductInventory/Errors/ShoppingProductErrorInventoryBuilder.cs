using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Errors;

public class ShoppingProductErrorInventoryBuilder : IError
{
    public const string SHOPPINGPRODUCT_CREATION_ERROR = "ShoppingProductCreationErrorException";
    public const string SHOPPINGPRODUCT_NOT_FOUND_ERROR = "ShoppingProductNotFoundErrorException";
    public static Error ShoppingProductCreationException() => Error.CreateInstance(
        SHOPPINGPRODUCT_CREATION_ERROR,
        "Failed to create Server due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string SHOPPINGPRODUCT_UPDATE_ERROR = "ShoppingProductUpdateErrorException";

    public static Error ShoppingProductUpdateException() => Error.CreateInstance(
            SHOPPINGPRODUCT_UPDATE_ERROR,
            "Failed to update Server due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error ShoppingProductNotFoundException(int id) => Error.CreateInstance(
       SHOPPINGPRODUCT_NOT_FOUND_ERROR,
       $"ShoppingProduct with ID {id} was not found.",
       HttpStatusCode.NotFound);

}

