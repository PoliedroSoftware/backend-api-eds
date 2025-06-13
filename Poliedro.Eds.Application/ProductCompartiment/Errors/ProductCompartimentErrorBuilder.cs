using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.ProductCompartiment.Errors;

public class ProductCompartimentErrorBuilder : IError
{
    public const string PRODUCTCOMPARTIMENT_CREATION_ERROR = "ProductCompartimentCreationErrorException";
    public const string PRODUCTCOMPARTIMENT_NOT_FOUND_ERROR = "ProductCompartimentNotFoundErrorException";
    public static Error ProductCompartimentCreationException() => Error.CreateInstance(
        PRODUCTCOMPARTIMENT_CREATION_ERROR,
        "Failed to create Server due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string PRODUCTCOMPARTIMENT_UPDATE_ERROR = "ProductCompartimentUpdateErrorException";

    public static Error ProductCompartimentUpdateException() => Error.CreateInstance(
            PRODUCTCOMPARTIMENT_UPDATE_ERROR,
            "Failed to update ProductCompartiment due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error ProductCompartimentNotFoundException(int id) => Error.CreateInstance(
       PRODUCTCOMPARTIMENT_NOT_FOUND_ERROR,
       $"ProductCompartiment with ID {id} was not found.",
       HttpStatusCode.NotFound);

}

