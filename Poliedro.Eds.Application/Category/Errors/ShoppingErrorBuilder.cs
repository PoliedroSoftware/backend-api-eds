using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Category.Errors;

public class CategoryErrorBuilder : IError
{
    public const string CATEGORY_CREATION_ERROR = "CategoryCreationErrorException";
    public const string CATEGORY_NOT_FOUND_ERROR = "CategoryNotFoundErrorException";
    public static Error CategoryCreationException() => Error.CreateInstance(
        CATEGORY_CREATION_ERROR,
        "Failed to create Server due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string CATEGORY_UPDATE_ERROR = "CategoryUpdateErrorException";

    public static Error CategoryUpdateException() => Error.CreateInstance(
            CATEGORY_UPDATE_ERROR,
            "Failed to update Server due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error CategoryNotFoundException(int id) => Error.CreateInstance(
       CATEGORY_NOT_FOUND_ERROR,
       $"Category with ID {id} was not found.",
       HttpStatusCode.NotFound);

}

