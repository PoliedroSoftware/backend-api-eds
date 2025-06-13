using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.CompartimentCapacity.Errors;

public class CompartimentCapacityErrorBuilder : IError
{
    public const string COMPARTIMENTCAPACITY_CREATION_ERROR = "CompartimentCapacityCreationErrorException";
    public const string COMPARTIMENTCAPACITY_NOT_FOUND_ERROR = "CompartimentCapacityNotFoundErrorException";
    public static Error CompartimentCapacityCreationException() => Error.CreateInstance(
        COMPARTIMENTCAPACITY_CREATION_ERROR,
        "Failed to create Server due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string COMPARTIMENTCAPACITY_UPDATE_ERROR = "CompartimentCapacityUpdateErrorException";

    public static Error CompartimentCapacityUpdateException() => Error.CreateInstance(
            COMPARTIMENTCAPACITY_UPDATE_ERROR,
            "Failed to update CompartimentCapacity due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error CompartimentCapacityNotFoundException(int id) => Error.CreateInstance(
       COMPARTIMENTCAPACITY_NOT_FOUND_ERROR,
       $"CompartimentCapacity with ID {id} was not found.",
       HttpStatusCode.NotFound);
}

