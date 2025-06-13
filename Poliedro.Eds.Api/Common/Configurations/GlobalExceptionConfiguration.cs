using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Poliedro.Eds.Application.Common.Constants;

namespace Poliedro.Eds.Api.Common.Configurations;

public class GlobalExceptionConfiguration(ILogger<GlobalExceptionConfiguration> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        logger.LogError(context.Exception.Message);

        if (context.Exception is FluentValidation.ValidationException validationException)
        {
            CreateValidationError(context, validationException);
        }
        else
        {
            CreateDefaultUnhandledError(context);
        }
    }

    private static void CreateValidationError(ExceptionContext context, FluentValidation.ValidationException validationException)
    {
        var validationFailures = GetValidationFailures(validationException);
        if (validationFailures != null)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = TypeExeptionConstants.VALIDATION_ERROR,
                Detail = "One or more validation errors occurred."
            };

            problemDetails.Extensions.Add("errors", validationFailures
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()));

            context.Result = new BadRequestObjectResult(problemDetails);
            context.ExceptionHandled = true;
        }
        else
        {
            CreateDefaultUnhandledError(context);
        }


    }

    private static void CreateDefaultUnhandledError(ExceptionContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = TypeExeptionConstants.INTERNAL_SERVER_ERROR,
            Detail = "An error occurred while processing your request."
        };

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
        context.ExceptionHandled = true;
    }

    private static IEnumerable<ValidationFailure>? GetValidationFailures(FluentValidation.ValidationException validationException)
    {
        return validationException.Errors;
    }
}
