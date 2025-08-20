using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Business.Commands.CreateBusiness;
using Poliedro.Eds.Application.Business.Commands.UpdateBusiness;
using Poliedro.Eds.Application.Business.Dtos;
using Poliedro.Eds.Application.Business.Errors;
using Poliedro.Eds.Application.Business.Queries.GellAllBusiness;
using Poliedro.Eds.Application.Business.Queries.GetBusinessById;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Business.Exepction;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Business;

[Route("api/v1/business")]
[ApiController]
public class BusinessController(IMediator mediator) : ControllerBase
{
    [Authorize(Policy = "AdminOrIslander")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllBusinessQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }
        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllBusinessQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }
        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }

    [SwaggerOperation(Summary = "Get business")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(BusinessDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The specified business does not exist.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpGet("{id}")]
    public async Task<IResult> GetById([FromRoute] int id)
    {
        var getBusinessQuery = new GetBusinessByIdQuery(Id: id);

        var result = await mediator.Send(getBusinessQuery);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    [SwaggerOperation(
        Summary = "Create new Business")]
    [SwaggerResponse(StatusCodes.Status201Created, "The operation was successful.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpPost]

    public async Task<IResult> Create(
        [FromBody] CreateBusinessCommand createBusinessCommand)
    {
        var result = await mediator.Send(createBusinessCommand);
        return result.Match(onSuccess => TypedResults.Created());
    }

    [SwaggerOperation(Summary = "Update an existing Business")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The requested Business was not found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [HttpPut]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update([FromBody] UpdateBusinessCommand updateBusinessCommand)
    {
        try
        {
            var result = await mediator.Send(updateBusinessCommand);

            if (!result.IsSuccess)
            {
                var errorMessage = result.Error?.Description ?? "Unknown error";
                var errorType = result.Error?.GetType().Name;

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = 500,
                    type = errorType,
                    message = errorMessage
                });
            }

            return NoContent();
        }
        catch (BusinessDomainException ex)
        {
            return BadRequest(new
            {
                status = 400,
                type = "BusinessDomainException",
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                status = 500,
                type = "UnhandledException",
                message = ex.Message
            });
        }
    }
}
