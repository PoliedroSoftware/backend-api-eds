using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.Provider.Commands.CreateProvider;
using Poliedro.Eds.Application.Provider.Commands.UpdateProvider;
using Poliedro.Eds.Application.Provider.Dtos;
using Poliedro.Eds.Application.Provider.Errors;
using Poliedro.Eds.Application.Provider.Queries.GellAllProvider;
using Poliedro.Eds.Application.Provider.Queries.GetProviderById;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;
using Poliedro.Eds.Application.Capacity.Commands.CreateCapacity;
using Microsoft.AspNetCore.Authorization;

namespace Poliedro.Eds.Api.Controllers.v1.Provider;

[Route("api/v1/provider")]
[ApiController]
public class ProviderController(IMediator mediator) : ControllerBase
{
    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllProviderQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }
        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }

    [SwaggerOperation(Summary = "Get Provider")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(ProviderDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The specified provider does not exist.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("{id}")]
    public async Task<IResult> GetById([FromRoute] int id, [FromServices] IValidator<GetProviderByIdQuery> validator)
    {
        var getProviderQuery = new GetProviderByIdQuery(Id: id);

        //var validationResult = await validator.ValidateAsync(getProviderQuery);

        //if (!validationResult.IsValid)
        //{
        //    return TypedResults.BadRequest(validationResult.Errors);
        //}

        var result = await mediator.Send(getProviderQuery);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value)
        );
    }

    [SwaggerOperation(
        Summary = "Create new Provider")]
    [SwaggerResponse(StatusCodes.Status201Created, "The operation was successful.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]

    public async Task<IResult> Create(
        [FromBody] CreateProviderCommand createProviderCommand,
        [FromServices] IValidator<CreateProviderRequestDto> validator)
    {
        //var validationResult = await validator.ValidateAsync(createProviderCommand.Request);
        //if (!validationResult.IsValid) return TypedResults.BadRequest(validationResult.Errors);
        var result = await mediator.Send(createProviderCommand);
        return result.Match(onSuccess => TypedResults.Created());
    }

    [SwaggerOperation(Summary = "Update an existing Provider")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The requested Provider was not found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    public async Task<IActionResult> Update(
    [FromBody] UpdateProviderCommand updateProviderCommand,
    [FromServices] IValidator<UpdateProviderCommand> validator)
    {
        //var validationResult = await validator.ValidateAsync(updateProviderCommand);
        //if (!validationResult.IsValid)
        //{
        //    return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors));
        //}

        var result = await mediator.Send(updateProviderCommand);

        if (!result.IsSuccess)
        {
            if (result.Error is ProviderErrorBuilder)
            {
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
        }

        return NoContent();
    }
}