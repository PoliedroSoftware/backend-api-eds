using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.Eds.Commands.CreateEds;
using Poliedro.Eds.Application.Eds.Commands.UpdateEds;
using Poliedro.Eds.Application.Eds.Dtos;
using Poliedro.Eds.Application.Eds.Errors;
using Poliedro.Eds.Application.Eds.Queries.GellAllEds;
using Poliedro.Eds.Application.Eds.Queries.GetEdsById;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Eds;

[Route("api/v1/eds")]
[ApiController]
public class EdsController(IMediator mediator) : ControllerBase
{
    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllEdsQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }
        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }

    [SwaggerOperation(Summary = "Get eds")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(EdsDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The specified eds does not exist.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("{id}")]
    public async Task<IResult> GetById([FromRoute] int id, [FromServices] IValidator<GetEdsByIdQuery> validator)
    {
        var getEdsQuery = new GetEdsByIdQuery(Id: id);

        //var validationResult = await validator.ValidateAsync(getEdsQuery);

        //if (!validationResult.IsValid)
        //{
        //    return TypedResults.BadRequest(validationResult.Errors);
        //}

        var result = await mediator.Send(getEdsQuery);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value)
        );
    }

    [SwaggerOperation(
        Summary = "Create new Eds")]
    [SwaggerResponse(StatusCodes.Status201Created, "The operation was successful.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]

    public async Task<IResult> Create(
            [FromBody] CreateEdsCommand createEdsCommand,
            [FromServices] IValidator<CreateEdsRequestDto> validator)
    {
        //var validationResult = await validator.ValidateAsync(createEdsCommand.Request);
        //if (!validationResult.IsValid) return TypedResults.BadRequest(validationResult.Errors);
        var result = await mediator.Send(createEdsCommand);
        return result.Match(onSuccess => TypedResults.Created());
    }

    [SwaggerOperation(Summary = "Update an existing Eds")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The requested Eds was not found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    public async Task<IActionResult> Update(
    [FromBody] UpdateEdsCommand updateEdsCommand,
    [FromServices] IValidator<UpdateEdsCommand> validator)
    {
        //var validationResult = await validator.ValidateAsync(updateEdsCommand);
        //if (!validationResult.IsValid)
        //{
        //    return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors));
        //}

        var result = await mediator.Send(updateEdsCommand);

        if (!result.IsSuccess)
        {
            if (result.Error is EdsErrorBuilder)
            {
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
        }

        return NoContent();
    }
}
