using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Hose.Errors
{
    public static class LastAccumulatedErrorBuilder
    {
        public const string LAST_ACCUMULATED_NOT_FOUND_ERROR = "LastAccumulatedNotFoundErrorException";

        public static Error LastAccumulatedNotFoundException(double gallons, double amount) => Error.CreateInstance(
            LAST_ACCUMULATED_NOT_FOUND_ERROR,
            $"Last accumulated data with gallons {gallons} and amount {amount} was not found.",
            HttpStatusCode.NotFound
        );
    }
}
