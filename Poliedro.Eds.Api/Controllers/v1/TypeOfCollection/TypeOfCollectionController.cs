using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.TypeOfCollection.Commands.CreateTypeOfCollection;
using Poliedro.Eds.Application.TypeOfCollection.Commands.UpdateTypeOfCollection;
using Poliedro.Eds.Application.TypeOfCollection.Dtos;
using Poliedro.Eds.Application.TypeOfCollection.Errors;
using Poliedro.Eds.Application.TypeOfCollection.Queries.GellAllTypeOfCollection;
using Poliedro.Eds.Application.TypeOfCollection.Queries.GetTypeOfCollectionById;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Islender
{
    [Route("api/v1/type-of-collection")]
    [ApiController]
    public class TypeOfCollectionController(IMediator mediator) : ControllerBase
    {
        [Authorize(Policy = "AdminOrIslander")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var data = await mediator.Send(new GellAllTypeOfCollectionQuery (new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
            if (data is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
            }
            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
        }

        [SwaggerOperation(Summary = "Get TypeOfCollection")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(TypeOfCollectionDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The specified TypeOfCollection does not exist.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOrIslander")]
        [HttpGet("{id}")]
        public async Task<IResult> GetById([FromRoute] int id, [FromServices] IValidator<GetTypeOfCollectionByIdQuery> validator)
        {
            var getTypeOfCollectionQuery = new GetTypeOfCollectionByIdQuery(Id : id );

            //var validationResult = await validator.ValidateAsync(getTypeOfCollectionQuery);

            //if (!validationResult.IsValid)
            //{
            //    return TypedResults.BadRequest(validationResult.Errors);
            //}

            var result = await mediator.Send(getTypeOfCollectionQuery);

            return result.Match(
                onSuccess => TypedResults.Ok(result.Value)
            );
        }

        [SwaggerOperation(
            Summary = "Create new TypeOfCollection")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize]
        [HttpPost]

        public async Task<IResult> Create(
            [FromBody] CreateTypeOfCollectionCommand createTypeOfCollectionCommand, 
            [FromServices] IValidator<CreateTypeOfCollectionRequestDto> validator)
        {
            //var validationResult = await validator.ValidateAsync(createTypeOfCollectionCommand.Request);
            //if (!validationResult.IsValid) return TypedResults.BadRequest(validationResult.Errors);
            var result = await mediator.Send(createTypeOfCollectionCommand);
            return result.Match(onSuccess => TypedResults.Created());
        }

        [SwaggerOperation(Summary = "Update an existing TypeOfCollection")]
        [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The requested TypeOfCollection was not found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public async Task<IActionResult> Update(
     [FromBody] UpdateTypeOfCollectionCommand updateTypeOfCollectionCommand,
     [FromServices] IValidator<UpdateTypeOfCollectionCommand> validator)
        {
            //var validationResult = await validator.ValidateAsync(updateTypeOfCollectionCommand);
            //if (!validationResult.IsValid)
            //{
            //    return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors));
            //}

            var result = await mediator.Send(updateTypeOfCollectionCommand);

            if (!result.IsSuccess)
            {
                if (result.Error is TypeOfCollectionErrorBuilder)
                {
                    return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
            }

            return NoContent();
        }
    }
}

