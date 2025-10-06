using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.CompartimentCapacity.Commands.CreateCompartimentCapacity;
using Poliedro.Eds.Application.CompartimentCapacity.Commands.UpdateCompartimentCapacity;
using Poliedro.Eds.Application.CompartimentCapacity.Dtos;
using Poliedro.Eds.Application.CompartimentCapacity.Errors;
using Poliedro.Eds.Application.CompartimentCapacity.Queries.GellAllCompartimentCapacity;
using Poliedro.Eds.Application.CompartimentCapacity.Queries.GetCompartimentCapacityById;
using Poliedro.Eds.Domain;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Islender
{
    [Route("api/v1/compartiment-capacity")]
    [ApiController]
    public class CompartimentCapacityController(IMediator mediator) : ControllerBase
    {
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var data = await mediator.Send(new GellAllCompartimentCapacityQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
            if (data is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
            }
            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
        }

        [SwaggerOperation(Summary = "Get CompartimentCapacity")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(CompartimentCapacityDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The specified CompartimentCapacity does not exist.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public async Task<IResult> GetById([FromRoute] int id)
        {
            var getCompartimentCapacityQuery = new GetCompartimentCapacityByIdQuery(Id: id);

            var result = await mediator.Send(getCompartimentCapacityQuery);

            return result.Match(
                onSuccess => TypedResults.Ok(result.Value),
                onFailure => TypedResults.BadRequest(onFailure)
            );
        }

        [SwaggerOperation(
            Summary = "Create new CompartimentCapacity")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]

        public async Task<IResult> Create([FromBody] CreateCompartimentCapacityCommand createCompartimentCapacityCommand)
        {
            var result = await mediator.Send(createCompartimentCapacityCommand);
            return result.Match(onSuccess => TypedResults.Created());
        }

        [SwaggerOperation(Summary = "Update an existing CompartimentCapacity")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The requested CompartimentCapacity was not found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCompartimentCapacityCommand updateCompartimentCapacityCommand)
        {
            var result = await mediator.Send(updateCompartimentCapacityCommand);

            if (!result.IsSuccess)
            {
                if (result.Error is CompartimentCapacityErrorBuilder)
                {
                    return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
            }

            return NoContent();
        }
    }
}

