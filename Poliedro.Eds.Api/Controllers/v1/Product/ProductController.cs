using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.Court.Errors;
using Poliedro.Eds.Application.Product.Commands.CreateProduct;
using Poliedro.Eds.Application.Product.Commands.UpdateProduct;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Application.Product.Queries.GellAllProduct;
using Poliedro.Eds.Application.Product.Queries.GetProductById;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Islender
{
    [Route("api/v1/product")]
    [ApiController]
    public class ProductController(IMediator mediator) : ControllerBase
    {
        [Authorize(Policy = "AdminOrIslander")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var data = await mediator.Send(new GellAllProductQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
            if (data is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
            }
            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
        }

        [SwaggerOperation(Summary = "Get Product")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(ProductDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The specified Product does not exist.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public async Task<IResult> GetById([FromRoute] int id)
        {
            var getProductQuery = new GetProductByIdQuery(Id: id);

            var result = await mediator.Send(getProductQuery);

            return result.Match(
                onSuccess => TypedResults.Ok(result.Value),
                onFailure => TypedResults.BadRequest(onFailure)
            );
        }

        [SwaggerOperation(
            Summary = "Create new Product")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]

        public async Task<IResult> Create([FromBody] CreateProductCommand createProductCommand)
        {
            var result = await mediator.Send(createProductCommand);
            return result.Match(onSuccess => TypedResults.Created());
        }

        [SwaggerOperation(Summary = "Update an existing Product")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The requested Product was not found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProductCommand updateProductCommand)
        {
            var result = await mediator.Send(updateProductCommand);

            if (!result.IsSuccess)
            {
                if (result.Error is CourtErrorBuilder)
                {
                    return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
            }

            return NoContent();
        }
    }
}

