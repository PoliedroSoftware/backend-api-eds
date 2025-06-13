using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.Expenditures.Commands.CreateExpenditures;
using Poliedro.Eds.Application.Expenditures.Commands.UpdateExpenditures;
using Poliedro.Eds.Application.Expenditures.Dtos;
using Poliedro.Eds.Application.Expenditures.Errors;
using Poliedro.Eds.Application.Expenditures.Queries.GellAllExpenditures;
using Poliedro.Eds.Application.Expenditures.Queries.GetExpendituresById;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Islender
{
    [Route("api/v1/expenditures")]
    [ApiController]
    public class ExpendituresController(IMediator mediator) : ControllerBase
    {
        [Authorize(Policy = "AdminOrIslander")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var data = await mediator.Send(new GellAllExpendituresQuery (new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
            if (data is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
            }
            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
        }

        [SwaggerOperation(Summary = "Get Expenditures")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(ExpendituresDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The specified Expenditures does not exist.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOrIslander")]
        [HttpGet("{id}")]
        public async Task<IResult> GetById([FromRoute] int id, [FromServices] IValidator<GetExpendituresByIdQuery> validator)
        {
            var getExpendituresQuery = new GetExpendituresByIdQuery(Id : id );

            //var validationResult = await validator.ValidateAsync(getExpendituresQuery);

            //if (!validationResult.IsValid)
            //{
            //    return TypedResults.BadRequest(validationResult.Errors);
            //}

            var result = await mediator.Send(getExpendituresQuery);

            return result.Match(
                onSuccess => TypedResults.Ok(result.Value)
            );
        }

        [SwaggerOperation(
            Summary = "Create new Expenditures")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOrIslander")]
        [HttpPost]

        public async Task<IResult> Create(
            [FromBody] CreateExpendituresCommand createExpendituresCommand, 
            [FromServices] IValidator<CreateExpendituresRequestDto> validator)
        {
            //var validationResult = await validator.ValidateAsync(createExpendituresCommand.Request);
            //if (!validationResult.IsValid) return TypedResults.BadRequest(validationResult.Errors);
            var result = await mediator.Send(createExpendituresCommand);
            return result.Match(onSuccess => TypedResults.Created());
        }

        [SwaggerOperation(Summary = "Update an existing Expenditures")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The requested Expenditures was not found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOrIslander")]
        [HttpPut]
        public async Task<IActionResult> Update(
     [FromBody] UpdateExpendituresCommand updateExpendituresCommand,
     [FromServices] IValidator<UpdateExpendituresCommand> validator)
        {
            //var validationResult = await validator.ValidateAsync(updateExpendituresCommand);
            //if (!validationResult.IsValid)
            //{
            //    return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors));
            //}

            var result = await mediator.Send(updateExpendituresCommand);

            if (!result.IsSuccess)
            {
                if (result.Error is ExpendituresErrorBuilder)
                {
                    return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
            }

            return NoContent();
        }
    }
}

