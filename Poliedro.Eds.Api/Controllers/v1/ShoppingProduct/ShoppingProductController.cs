using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;
using Poliedro.Eds.Application.ShoppingProduct.Commands.UpdateShoppingProduct;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Application.ShoppingProduct.Errors;
using Poliedro.Eds.Application.ShoppingProduct.Queries.GellAllShoppingProduct;
using Poliedro.Eds.Application.ShoppingProduct.Queries.GetShoppingProductById;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.ShoppingProduct
{
    [Route("api/v1/shopping-product")]
    [ApiController]
    public class ShoppingProductController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Retrieves all ShoppingProduct records.
        /// </summary>
        /// <returns>A status code indicating the result of the operation and the list of client billing electronic records.</returns>
        /// <response code="200">Returns the list of client billing electronic records.</response>
        /// <response code="404">Returns when there are no client billing electronic records found.</response>
        /// <response code="500">Returns when there is an Internal Server Error.</response>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingProductDto>>> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var data = await mediator.Send(new GellAllShoppingProductQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
            if (data is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
        }
        [SwaggerOperation(Summary = "Get ShoppingProduct")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(ShoppingProductDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The specified ShoppingProduct does not exist.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public async Task<IResult> GetById([FromRoute] int id, [FromServices] IValidator<GetShoppingProductByIdQuery> validator)
        {
            var getShoppingProductQuery = new GetShoppingProductByIdQuery(Id: id);

            //var validationResult = await validator.ValidateAsync(getShoppingProductQuery);

            //if (!validationResult.IsValid)
            //{
            //    return TypedResults.BadRequest(validationResult.Errors);
            //}

            var result = await mediator.Send(getShoppingProductQuery);

            return result.Match(
                onSuccess => TypedResults.Ok(result.Value)
            );
        }

        [SwaggerOperation(
            Summary = "Create new ShoppingProduct")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]

        public async Task<IResult> Create(
            [FromBody] CreateShoppingProductCommand createShoppingProductCommand,
            [FromServices] IValidator<CreateShoppingProductRequestDto> validator)

        {

            var validationResult = await validator.ValidateAsync(createShoppingProductCommand.Request);
            if (!validationResult.IsValid)
            {
                var mensajes = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return TypedResults.BadRequest(mensajes);
            }

            var result = await mediator.Send(createShoppingProductCommand);
            return result.Match(
                 onSuccess => TypedResults.Created()
             );
        }

        [SwaggerOperation(Summary = "Update an existing ShoppingProduct")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The requested ShoppingProduct was not found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public async Task<IActionResult> Update(
        [FromBody] UpdateShoppingProductCommand updateShoppingProductCommand,
        [FromServices] IValidator<UpdateShoppingProductCommand> validator)
        {
            var validationResult = await validator.ValidateAsync(updateShoppingProductCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors));
            }

            var result = await mediator.Send(updateShoppingProductCommand);

            if (!result.IsSuccess)
            {
                if (result.Error is ShoppingProductErrorBuilder)
                {
                    return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
            }
            return NoContent();
        }
    }
}
