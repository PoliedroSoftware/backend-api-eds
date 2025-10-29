using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.Islander.Commands.CreateIslander;
using Poliedro.Eds.Application.Islander.Commands.UpdateIslander;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Application.Islander.Errors;
using Poliedro.Eds.Application.Islander.Queries.GellAllIslander;
using Poliedro.Eds.Application.Islander.Queries.GetIslanderById;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Swashbuckle.AspNetCore.Annotations;
using YamlDotNet.Core;

namespace Poliedro.Eds.Api.Controllers.v1.Islender
{
    [Route("api/v1/islander")]
    [ApiController]
    public class IslanderController(IMediator mediator) : ControllerBase
    {
        [Authorize(Policy = "AdminOrIslander")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var data = await mediator.Send(new GellAllIslanderQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
            if (data is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
            }
            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
        }

        [SwaggerOperation(Summary = "Get islander")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(IslanderDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The specified islander does not exist.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOrIslander")]
        [HttpGet("{id}")]
        public async Task<IResult> GetById([FromRoute] int id)
        {
            var getIslanderQuery = new GetIslanderByIdQuery(Id: id);

            var result = await mediator.Send(getIslanderQuery);

            return result.Match(
                onSuccess => TypedResults.Ok(result.Value),
                onFailure => TypedResults.BadRequest(onFailure)
            );
        }

        [SwaggerOperation(
            Summary = "Create new Islander")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOrIslander")]
        [HttpPost]
        public async Task<IResult> Create([FromBody] CreateIslanderCommand createIslanderCommand)
        {
            int? idEds = null;
            var claimVal = HttpContext.User?.FindFirst("id_eds")?.Value ?? HttpContext.User?.FindFirst("idEds")?.Value;
            if (!string.IsNullOrEmpty(claimVal) && int.TryParse(claimVal, out var parsedClaim))
                idEds = parsedClaim;
            else if (HttpContext.Items["id_eds"] != null && int.TryParse(HttpContext.Items["id_eds"]?.ToString(), out var parsedItem))
                idEds = parsedItem;

            var req = createIslanderCommand.Request;
            var command = new CreateIslanderCommand(req, createIslanderCommand.NameClaimToken, idEds);
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }

        [SwaggerOperation(Summary = "Update an existing Islander")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The requested Islander was not found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateIslanderCommand updateIslanderCommand)
        {
            var result = await mediator.Send(updateIslanderCommand);

            if (!result.IsSuccess)
            {
                if (result.Error is IslanderErrorBuilder)
                {
                    return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
            }

            return NoContent();
        }
    }
}

