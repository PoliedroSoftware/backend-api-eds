using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.Compartiment.Commands.CreateCompartiment;
using Poliedro.Eds.Application.Compartiment.Commands.UpdateCompartiment;
using Poliedro.Eds.Application.Compartiment.Dtos;
using Poliedro.Eds.Application.Compartiment.Errors;
using Poliedro.Eds.Application.Compartiment.Queries.GellAllCompartiment;
using Poliedro.Eds.Application.Compartiment.Queries.GetCompartimentById;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Compartiment;

[Route("api/v1/compartiment")]
[ApiController]
public class CompartimentController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retrieves all Compartiment records.
    /// </summary>
    /// <returns>A status code indicating the result of the operation and the list of client billing electronic records.</returns>
    /// <response code="200">Returns the list of client billing electronic records.</response>
    /// <response code="404">Returns when there are no client billing electronic records found.</response>
    /// <response code="500">Returns when there is an Internal Server Error.</response>
    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompartimentDto>>> GetAll([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllCompartimentQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }

        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }
    [SwaggerOperation(Summary = "Get Compartiment")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(CompartimentDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The specified Compartiment does not exist.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("{id}")]
    public async Task<IResult> GetById([FromRoute] int id, [FromServices] IValidator<GetCompartimentByIdQuery> validator)
    {
        var getCompartimentQuery = new GetCompartimentByIdQuery(Id: id);

        //var validationResult = await validator.ValidateAsync(getCompartimentQuery);

        //if (!validationResult.IsValid)
        //{
        //    return TypedResults.BadRequest(validationResult.Errors);
        //}

        var result = await mediator.Send(getCompartimentQuery);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value)
        );
    }

    [SwaggerOperation(
        Summary = "Create new Compartiment")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]

    public async Task<IResult> Create(
            [FromBody] CreateCompartimentCommand createCompartimentCommand,
            [FromServices] IValidator<CreateCompartimentRequestDto> validator)

    {
        //var validationResult = await validator.ValidateAsync(createCompartimentCommand.Request);
        //if (!validationResult.IsValid) return TypedResults.BadRequest(validationResult.Errors);
        var result = await mediator.Send(createCompartimentCommand);
        return result.Match(
             onSuccess => TypedResults.Created()
         );
    }

    [SwaggerOperation(Summary = "Update an existing Compartiment")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The requested Compartiment was not found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    public async Task<IActionResult> Update(
 [FromBody] UpdateCompartimentCommand updateCompartimentCommand,
 [FromServices] IValidator<UpdateCompartimentCommand> validator)
    {
        //var validationResult = await validator.ValidateAsync(updateCompartimentCommand);
        //if (!validationResult.IsValid)
        //{
        //    return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors));
        //}

        var result = await mediator.Send(updateCompartimentCommand);

        if (!result.IsSuccess)
        {
            if (result.Error is CompartimentErrorBuilder)
            {
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
        }
        return NoContent();
    }
}
