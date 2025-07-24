using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.Court.Commands.CreateCourt;
using Poliedro.Eds.Application.Court.Commands.UpdateCourt;
using Poliedro.Eds.Application.Court.Dtos;
using Poliedro.Eds.Application.Court.Errors;
using Poliedro.Eds.Application.Court.Queris.GetCourtById;
using Poliedro.Eds.Application.Court.Queris.GetCourtList;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Court;

[Route("api/v1/[controller]")]
[ApiController]
public class CourtController(IMediator mediator) : ControllerBase
{
    [SwaggerOperation(Summary = "Get Court")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(CourtDto))]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("{id}")]
    public async Task<IResult> GetById([FromRoute] int id)
    {
        var getCourtQuery = new GetCourtByIdQuery(Id: id);

        var result = await mediator.Send(getCourtQuery);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    [SwaggerOperation(Summary = "Create new Court")]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpPost]
    public async Task<IResult> Create(
        [FromBody] CreateCourtCommand createCourtCommand)
    {
        var result = await mediator.Send(createCourtCommand);
        return result.Match(onSuccess => TypedResults.Created());
    }

    [SwaggerOperation(Summary = "Update an existing Court")]
    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    public async Task<IActionResult> Update(
    [FromBody] UpdateCourtCommand updateCourtCommand,
    [FromServices] IValidator<UpdateCourtCommand> validator)
    {
        //var validationResult = await validator.ValidateAsync(updateCourtCommand);
        //if (!validationResult.IsValid)
        //{
        //    return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors));
        //}

        var result = await mediator.Send(updateCourtCommand);

        if (!result.IsSuccess)
        {
            if (result.Error is CourtErrorBuilder)
            {
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
        }
        return Ok(ResponseApiService.Response(StatusCodes.Status200OK));
    }


    [SwaggerOperation(Summary = "Get all Courts")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(IEnumerable<CourtListResponseDto>))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpGet]
    public async Task<IResult> GetAll(
    [FromQuery] PaginationParams paginationParams,
    [FromServices] IValidator<GetCourtsListQuery> validator)
    {
        try
        {
            var query = new GetCourtsListQuery(paginationParams);

            var validation = await validator.ValidateAsync(query);
            if (!validation.IsValid)
                return TypedResults.BadRequest(validation.Errors);

            var result = await mediator.Send(query);
            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem($"Error interno del servidor: {ex.Message}");
        }
    }

}