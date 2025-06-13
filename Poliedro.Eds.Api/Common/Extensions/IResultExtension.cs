using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Poliedro.Eds.Api.Common.Extensions;

public class ErrorResult : IResult
{
    private readonly ProblemDetails problemDetails;

    public HttpStatusCode StatusCode => (HttpStatusCode)problemDetails.Status!;

    public ErrorResult(string errorCode, string errorDescription, HttpStatusCode httpStatusCode)
    {
        problemDetails = new ProblemDetails
        {
            Status = (int)httpStatusCode,
            Type = errorCode,
            Detail = errorDescription
        };

    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails).ConfigureAwait(false);
    }
}
