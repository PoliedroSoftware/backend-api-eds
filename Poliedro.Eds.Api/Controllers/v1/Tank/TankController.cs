using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.Tank.Commands.CreateTank;
using Poliedro.Eds.Application.Tank.Commands.UpdateTank;
using Poliedro.Eds.Application.Tank.Dtos;
using Poliedro.Eds.Application.Tank.Errors;
using Poliedro.Eds.Application.Tank.Queries.GellAllTank;
using Poliedro.Eds.Application.Tank.Queries.GetTankById;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Tank
{
    [Route("api/v1/tank")]
    [ApiController]
    public class TankController(IMediator mediator) : ControllerBase
    {
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var data = await mediator.Send(new GellAllTankQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
            if (data is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
            }
            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
        }

        [SwaggerOperation(Summary = "Get Tank")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(TankDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The specified Tank does not exist.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public async Task<IResult> GetById([FromRoute] int id, [FromServices] IValidator<GetTankByIdQuery> validator)
        {
            var getTankQuery = new GetTankByIdQuery(Id: id);

            //var validationResult = await validator.ValidateAsync(getTankQuery);

            //if (!validationResult.IsValid)
            //{
            //    return TypedResults.BadRequest(validationResult.Errors);
            //}

            var result = await mediator.Send(getTankQuery);

            return result.Match(
                onSuccess => TypedResults.Ok(result.Value)
            );
        }

        [SwaggerOperation(
            Summary = "Create new Tank")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]

        public async Task<IResult> Create(
           [FromBody] CreateTankCommand createTankCommand,
           [FromServices] IValidator<CreateTankRequestDto> validator)

        {
            //var validationResult = await validator.ValidateAsync(createTankCommand.Request);
            //if (!validationResult.IsValid) return TypedResults.BadRequest(validationResult.Errors);
            var result = await mediator.Send(createTankCommand);
            return result.Match(
                 onSuccess => TypedResults.Created()
             );
        }

        [SwaggerOperation(Summary = "Update an existing Tank")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The requested Tank was not found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public async Task<IActionResult> Update(
        [FromBody] UpdateTankCommand updateTankCommand,
        [FromServices] IValidator<UpdateTankCommand> validator)
        {
            var validationResult = await validator.ValidateAsync(updateTankCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors));
            }

            var result = await mediator.Send(updateTankCommand);

            if (!result.IsSuccess)
            {
                if (result.Error is TankErrorBuilder)
                {
                    return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
            }

            return NoContent();
        }
    }
}

