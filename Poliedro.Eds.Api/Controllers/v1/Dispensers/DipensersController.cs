using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.Dispensers.Commands.CreateDispensers;
using Poliedro.Eds.Application.Dispensers.Commands.UpdateDispensers;
using Poliedro.Eds.Application.Dispensers.Dtos;
using Poliedro.Eds.Application.Dispensers.Errors;
using Poliedro.Eds.Application.Dispensers.Queries.GellAllDispensers;
using Poliedro.Eds.Application.Dispensers.Queries.GetDispensersById;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Dispensers;

[Route("api/v1/dispensers")]
[ApiController]
public class DispensersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retrieves all Disepnsers records.
    /// </summary>
    /// <returns>A status code indicating the result of the operation and the list of client billing electronic records.</returns>
    /// <response code="200">Returns the list of client billing electronic records.</response>
    /// <response code="404">Returns when there are no client billing electronic records found.</response>
    /// <response code="500">Returns when there is an Internal Server Error.</response>
    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DispensersDto>>> GetAll([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllDispensersQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }

        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }
    [SwaggerOperation(Summary = "Get Dispensers")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(DispensersDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The specified Dispensers does not exist.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("{id}")]
    public async Task<IResult> GetById([FromRoute] int id, [FromServices] IValidator<GetDispensersByIdQuery> validator)
    {
        var getDispenserseQuery = new GetDispensersByIdQuery(Id: id);

        //var validationResult = await validator.ValidateAsync(getDispenserseQuery);

        //if (!validationResult.IsValid)
        //{
        //    return TypedResults.BadRequest(validationResult.Errors);
        //}

        var result = await mediator.Send(getDispenserseQuery);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value)
        );
    }

    [SwaggerOperation(
        Summary = "Create new Dispensers")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]

    public async Task<IResult> Create(
            [FromBody] CreateDispensersCommand createDispensersCommand,
            [FromServices] IValidator<CreateDispensersRequestDto> validator)

    {
            //var validationResult = await validator.ValidateAsync(createDispensersCommand.Request);
            //if (!validationResult.IsValid) return TypedResults.BadRequest(validationResult.Errors);
        var result = await mediator.Send(createDispensersCommand);
            return result.Match(
                 onSuccess => TypedResults.Created()
             );
    }

    [SwaggerOperation(Summary = "Update an existing Dispensers")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The requested Dispensers was not found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    public async Task<IActionResult> Update(
 [FromBody] UpdateDispensersCommand updateDispensersCommand,
 [FromServices] IValidator<UpdateDispensersCommand> validator)
    {
        //var validationResult = await validator.ValidateAsync(updateDispensersCommand);
        //if (!validationResult.IsValid)
        //{
        //    return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors));
        //}

        var result = await mediator.Send(updateDispensersCommand);

        if (!result.IsSuccess)
        {
            if (result.Error is DispensersErrorBuilder)
            {
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
        }
        return NoContent();
    }
}
