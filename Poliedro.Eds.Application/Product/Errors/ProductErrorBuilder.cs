using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Product.Errors;

public class ProductErrorBuilder : IError
{
    public const string PRODUCT_CREATION_ERROR = "ProductCreationErrorException";
    public const string PRODUCT_NOT_FOUND_ERROR = "ProductNotFoundErrorException";
    public static Error ProductCreationException() => Error.CreateInstance(
        PRODUCT_CREATION_ERROR,
        "Failed to create Server due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string PRODUCT_UPDATE_ERROR = "ProductUpdateErrorException";

    public static Error ProductUpdateException() => Error.CreateInstance(
            PRODUCT_UPDATE_ERROR,
            "Failed to update Product due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error ProductNotFoundException(int id) => Error.CreateInstance(
       PRODUCT_NOT_FOUND_ERROR,
       $"Product with ID {id} was not found.",
       HttpStatusCode.NotFound);

}

