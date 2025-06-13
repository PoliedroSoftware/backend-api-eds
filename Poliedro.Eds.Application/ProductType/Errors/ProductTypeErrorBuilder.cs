using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.ProductType.Errors;

public class ProductTypeErrorBuilder : IError
{
    public const string PRODUCTTYPE_CREATION_ERROR = "ProductTypeCreationErrorException";
    public const string PRODUCTTYPE_NOT_FOUND_ERROR = "ProductTypeNotFoundErrorException";
    public static Error ProductTypeCreationException() => Error.CreateInstance(
        PRODUCTTYPE_CREATION_ERROR,
        "Failed to create Server due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string PRODUCTTYPE_UPDATE_ERROR = "ProductTypeUpdateErrorException";

    public static Error ProductTypeUpdateException() => Error.CreateInstance(
            PRODUCTTYPE_UPDATE_ERROR,
            "Failed to update ProductType due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error ProductTypeNotFoundException(int id) => Error.CreateInstance(
       PRODUCTTYPE_NOT_FOUND_ERROR,
       $"ProductType with ID {id} was not found.",
       HttpStatusCode.NotFound);

}

