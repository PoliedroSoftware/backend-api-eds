using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Application.ShoppingProductInventory.Commands.CreateShoppingProductInventory;
using Poliedro.Eds.Application.ShoppingProductInventory.Dtos;
using Poliedro.Eds.Application.ShoppingProductInventory.Queries.GellAllShoppingProductInventory;
using Poliedro.Eds.Application.ShoppingProductInventory.Queries.GetShoppingProductInventoryById;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.ShoppingProductInventory
{
    [Route("api/v1/shopping-product-inventory")]
    [ApiController]
    public class ShoppingProductInventoryController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Retrieves all ShoppingProductInventory records.
        /// </summary>
        /// <param name="paginationParams"></param>
        /// <returns></returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingProductInventoryDto>>> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var data = await mediator.Send(new GellAllShoppingProductInventoryQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
            if (data is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
        }

       
        [SwaggerOperation(Summary = "Create new ShoppingInventoryProduct")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IResult> Create([FromBody] CreateShoppingProductInventoryCommand createShoppingProductInventoryCommand,
            [FromServices] IValidator<CreateShoppingProductInventoryRequestDto> validator)
        {
            //var validationResult = await validator.ValidateAsync(createShoppingProductInventoryCommand.Request);
            //if (!validationResult.IsValid) return TypedResults.BadRequest(validationResult.Errors);
            var result = await mediator.Send(createShoppingProductInventoryCommand);
            return result.Match(
                 onSuccess => TypedResults.Created()
             );
        }



        [SwaggerOperation(Summary = "Get ShoppingProductInventory")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(ShoppingProductDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The specified ShoppingProduct does not exist.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public async Task<IResult> GetShopingProductInventoryByIdShopping([FromRoute] int id, [FromServices] IValidator<GetShoppingProductInventoryByIdQuery> validator)
        {
            var getShoppingProductQuery = new GetShoppingProductInventoryByIdQuery(Id: id);

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


        




    }
}
