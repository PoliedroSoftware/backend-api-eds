using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.TypeOfCollection.Errors;

public class TypeOfCollectionErrorBuilder : IError
{
    public const string TYPEOFCOLLECTION_CREATION_ERROR = "TypeOfCollectionCreationErrorException";
    public const string TYPEOFCOLLECTION_NOT_FOUND_ERROR = "TypeOfCollectionNotFoundErrorException";
    public static Error TypeOfCollectionCreationException() => Error.CreateInstance(
        TYPEOFCOLLECTION_CREATION_ERROR,
        "Failed to create Server due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string TYPEOFCOLLECTION_UPDATE_ERROR = "TypeOfCollectionUpdateErrorException";

    public static Error TypeOfCollectionUpdateException() => Error.CreateInstance(
            TYPEOFCOLLECTION_UPDATE_ERROR,
            "Failed to update TypeOfCollection due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error TypeOfCollectionNotFoundException(int id) => Error.CreateInstance(
       TYPEOFCOLLECTION_NOT_FOUND_ERROR,
       $"TypeOfCollection with ID {id} was not found.",
       HttpStatusCode.NotFound);
}

