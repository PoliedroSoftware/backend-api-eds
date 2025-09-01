using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.StrongBox.Commands;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetById;
using Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetList;
using Poliedro.Eds.Domain.Common.Models;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.StrongBox
{
    [Route("api/v1/strongbox")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class StrongBoxController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(Summary = "Get all StrongBox records")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(List<StrongBoxDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ProblemDetails))]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaginationParams paginationParams)
        {
            var query = new StrongBoxGetList(Page: paginationParams.PageNumber,
                PageSize: paginationParams.PageSize);

            var data = await mediator.Send(query);
            return ApiResponse(data, StatusCodes.Status200OK, StatusCodes.Status404NotFound);
        }

        [SwaggerOperation(Summary = "Get StrongBox record by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(StrongBoxDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "Service Unavailable", typeof(ProblemDetails))]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById([FromRoute] long id)
        {
            var dto = await mediator.Send(new StrongBoxGetId(id));
            return ApiResponse(dto, StatusCodes.Status200OK, StatusCodes.Status404NotFound);
        }

        [SwaggerOperation(Summary = "Create a new Record (CORTE o RETIRO)")]
        [SwaggerResponse(StatusCodes.Status201Created, "Successful", typeof(StrongBoxDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict", typeof(ProblemDetails))]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StrongBoxCreateCommand command)
        {
            try
            {
                var created = await mediator.Send(command);
                return ApiResponse(created, StatusCodes.Status201Created, StatusCodes.Status400BadRequest);
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                return BadRequest(new ValidationProblemDetails(errors));
            }
            catch (InvalidCastException ex)
            {
                return Problem(title: "Invalid Cast Exception",
                    detail: ex.Message,
                    statusCode: 500);
            }
        }

        private IActionResult ApiResponse<T>(T? data, int successStatus, int notFoundStatus)
        {
            if (data == null || (data is ICollection<StrongBoxDto> list && list.Count == 0))
            {
                return StatusCode(notFoundStatus, ResponseApiService.Response(notFoundStatus));
            }
            return StatusCode(successStatus, ResponseApiService.Response(successStatus, data));
        }
    }
}

