using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Expenditures.Errors;

public class ExpendituresErrorBuilder : IError
{
    public const string EXPENDITURES_CREATION_ERROR = "ExpendituresCreationErrorException";
    public const string EXPENDITURES_NOT_FOUND_ERROR = "ExpendituresNotFoundErrorException";
    public static Error ExpendituresCreationException() => Error.CreateInstance(
        EXPENDITURES_CREATION_ERROR,
        "Failed to create Server due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string EXPENDITURES_UPDATE_ERROR = "ExpendituresUpdateErrorException";

    public static Error ExpendituresUpdateException() => Error.CreateInstance(
            EXPENDITURES_UPDATE_ERROR,
            "Failed to update Expenditures due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error ExpendituresNotFoundException(int id) => Error.CreateInstance(
       EXPENDITURES_NOT_FOUND_ERROR,
       $"Expenditures with ID {id} was not found.",
       HttpStatusCode.NotFound);
}

