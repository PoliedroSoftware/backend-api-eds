using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.Hose.Commands.CreateHose;
using Poliedro.Eds.Application.Hose.Commands.UpdateHose;
using Poliedro.Eds.Application.Hose.Dtos;
using Poliedro.Eds.Application.Hose.Errors;
using Poliedro.Eds.Application.Hose.Queries.GellAllHose;
using Poliedro.Eds.Application.Hose.Queries.GetHoseById;
using Poliedro.Eds.Application.Hose.Queries.GetLastAccumulated;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Hose.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Hose
{
    [Route("api/v1/hose")]
    [ApiController]
    public class HoseController(IMediator mediator) : ControllerBase
    {
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var data = await mediator.Send(new GellAllHoseQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
            if (data is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
            }
            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
        }

        [SwaggerOperation(Summary = "Get Hose")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(HoseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The specified Hose does not exist.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public async Task<IResult> GetById([FromRoute] int id, [FromServices] IValidator<GetHoseByIdQuery> validator)
        {
            var getHoseQuery = new GetHoseByIdQuery(Id: id);

            //var validationResult = await validator.ValidateAsync(getHoseQuery);

            //if (!validationResult.IsValid)
            //{
            //    return TypedResults.BadRequest(validationResult.Errors);
            //}

            var result = await mediator.Send(getHoseQuery);

            return result.Match(
                onSuccess => TypedResults.Ok(result.Value)
            );
        }

        [SwaggerOperation(Summary = "Get LastAccumulated")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(LastAccumulatedDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The specified Hose does not exist.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("last-accumulated")]
        public async Task<IActionResult> GetLastAccumulated(
            [FromQuery] int idDispenser,
            [FromQuery] int idHose,
            [FromServices] IValidator<GetLastAccumulatedQuery> validator)
        {
            var getLastAccumulatedQuery = new GetLastAccumulatedQuery(idDispenser, idHose);
            //var validationResult = await validator.ValidateAsync(getLastAccumulatedQuery);
            //if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var result = await mediator.Send(getLastAccumulatedQuery);
            if (!result.IsSuccess) return StatusCode((int)result.Error.HttpStatusCode, result.Error);
            return Ok(result.Value);
        }

        [SwaggerOperation(
            Summary = "Create new Hose")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]

        public async Task<IResult> Create(
            [FromBody] CreateHoseCommand createHoseCommand,
            [FromServices] IValidator<CreateHoseRequestDto> validator)

        {
            //var validationResult = await validator.ValidateAsync(createHoseCommand.Request);
            //if (!validationResult.IsValid) return TypedResults.BadRequest(validationResult.Errors);
            var result = await mediator.Send(createHoseCommand);
            return result.Match(
                 onSuccess => TypedResults.Created()
             );
        }

        [SwaggerOperation(Summary = "Update an existing Hose")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The requested Hose was not found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public async Task<IActionResult> Update(
     [FromBody] UpdateHoseCommand updateHoseCommand,
     [FromServices] IValidator<UpdateHoseCommand> validator)
        {
            //var validationResult = await validator.ValidateAsync(updateHoseCommand);
            //if (!validationResult.IsValid)
            //{
            //    return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors));
            //}

            var result = await mediator.Send(updateHoseCommand);

            if (!result.IsSuccess)
            {
                if (result.Error is HoseErrorBuilder)
                {
                    return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
            }

            return NoContent();
        }
    }
}

