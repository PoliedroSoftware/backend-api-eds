using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Category.Commands.CreateCategory;
using Poliedro.Eds.Application.Category.Commands.UpdateCategory;
using Poliedro.Eds.Application.Category.Dtos;
using Poliedro.Eds.Application.Category.Errors;
using Poliedro.Eds.Application.Category.Queries.GellAllCategory;
using Poliedro.Eds.Application.Category.Queries.GetCategoryById;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Category;

[Route("api/v1/category")]
[ApiController]
public class CategoryController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retrieves all Category records.
    /// </summary>
    /// <returns>A status code indicating the result of the operation and the list of client billing electronic records.</returns>
    /// <response code="200">Returns the list of client billing electronic records.</response>
    /// <response code="404">Returns when there are no client billing electronic records found.</response>
    /// <response code="500">Returns when there is an Internal Server Error.</response>
    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllCategoryQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }

        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }
    [SwaggerOperation(Summary = "Get Category")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(CategoryDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The specified Category does not exist.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("{id}")]
    public async Task<IResult> GetById([FromRoute] int id, [FromServices] IValidator<GetCategoryByIdQuery> validator)
    {
        var getCategoryQuery = new GetCategoryByIdQuery(Id: id);

        //var validationResult = await validator.ValidateAsync(getCategoryQuery);

        //if (!validationResult.IsValid)
        //{
        //    return TypedResults.BadRequest(validationResult.Errors);
        //}

        var result = await mediator.Send(getCategoryQuery);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value)
        );
    }

    [SwaggerOperation(
        Summary = "Create new Category")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]

    public async Task<IResult> Create(
            [FromBody] CreateCategoryCommand createCategoryCommand,
            [FromServices] IValidator<CreateCategoryRequestDto> validator)

    {
        //var validationResult = await validator.ValidateAsync(createCategoryCommand.Request);
        //if (!validationResult.IsValid) return TypedResults.BadRequest(validationResult.Errors);
        var result = await mediator.Send(createCategoryCommand);
        return result.Match(
             onSuccess => TypedResults.Created()
         );
    }

    [SwaggerOperation(Summary = "Update an existing Category")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The requested Category was not found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    public async Task<IActionResult> Update(
 [FromBody] UpdateCategoryCommand updateCategoryCommand,
 [FromServices] IValidator<UpdateCategoryCommand> validator)
    {
        //var validationResult = await validator.ValidateAsync(updateCategoryCommand);
        //if (!validationResult.IsValid)
        //{
        //    return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors));
        //}

        var result = await mediator.Send(updateCategoryCommand);

        if (!result.IsSuccess)
        {
            if (result.Error is CategoryErrorBuilder)
            {
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
        }
        return NoContent();
    }
}
