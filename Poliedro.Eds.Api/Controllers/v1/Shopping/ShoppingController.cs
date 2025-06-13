using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
using Poliedro.Eds.Application.Shopping.Commands.UpdateShopping;
using Poliedro.Eds.Application.Shopping.Dtos;
using Poliedro.Eds.Application.Shopping.Errors;
using Poliedro.Eds.Application.Shopping.Queries.GellAllShopping;
using Poliedro.Eds.Application.Shopping.Queries.GetShoppingById;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Shopping
{
    [Route("api/v1/shopping")]
    [ApiController]
    public class ShoppingController(IMediator mediator) : Controller
    {
        /// <summary>
        /// Retrieves all Shopping records.
        /// </summary>
        /// <returns>A status code indicating the result of the operation and the list of client billing electronic records.</returns>
        /// <response code="200">Returns the list of client billing electronic records.</response>
        /// <response code="404">Returns when there are no client billing electronic records found.</response>
        /// <response code="500">Returns when there is an Internal Server Error.</response>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingDto>>> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var data = await mediator.Send(new GellAllShoppingQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
            if (data is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
        }
        [SwaggerOperation(Summary = "Get Shopping")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(ShoppingDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The specified Shopping does not exist.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public async Task<IResult> GetById([FromRoute] int id, [FromServices] IValidator<GetShoppingByIdQuery> validator)
        {
            var getShoppingQuery = new GetShoppingByIdQuery(Id: id);

            //var validationResult = await validator.ValidateAsync(getShoppingQuery);

            //if (!validationResult.IsValid)
            //{
            //    return TypedResults.BadRequest(validationResult.Errors);
            //}

            var result = await mediator.Send(getShoppingQuery);

            return result.Match(
                onSuccess => TypedResults.Ok(result.Value)
            );
        }

        [SwaggerOperation(
            Summary = "Create new Shopping")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]

        public async Task<IResult> Create(
            [FromBody] CreateShoppingCommand createShoppingCommand,
            [FromServices] IValidator<CreateShoppingRequestDto> validator)

        {
            //var validationResult = await validator.ValidateAsync(createShoppingCommand.Request);
            //if (!validationResult.IsValid) return TypedResults.BadRequest(validationResult.Errors);
            var result = await mediator.Send(createShoppingCommand);
            return result.Match(
                 onSuccess => TypedResults.Created()
             );
        }

        [SwaggerOperation(Summary = "Update an existing Shopping")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The requested Shopping was not found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public async Task<IActionResult> Update(
        [FromBody] UpdateShoppingCommand updateShoppingCommand,
        [FromServices] IValidator<UpdateShoppingCommand> validator)
        {
            //var validationResult = await validator.ValidateAsync(updateShoppingCommand);
            //if (!validationResult.IsValid)
            //{
            //    return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors));
            //}

            var result = await mediator.Send(updateShoppingCommand);

            if (!result.IsSuccess)
            {
                if (result.Error is ShoppingErrorBuilder)
                {
                    return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
            }
            return NoContent();
        }
    }
}
