using System.Net;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Wizard.Errors;

public class SetupErrorBuilder : IError
{
    public const string SETUP_CREATION_ERROR = "SetupCreationErrorException";

    public static Error SetupCreationException(string? message = "") => Error.CreateInstance(
       SETUP_CREATION_ERROR,
        $"Failed to create Setup due to an internal error: {message}",
        HttpStatusCode.InternalServerError);
}
