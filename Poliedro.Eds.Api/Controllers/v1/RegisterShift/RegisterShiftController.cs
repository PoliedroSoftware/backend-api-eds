using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.RegisterShift.Commands.CreateRegisterShift;
using Poliedro.Eds.Application.RegisterShift.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.RegisterShift;

[Route("api/v1/registershift")]
[ApiController]

public class RegisterShiftController(IMediator mediator) : ControllerBase
{
    [SwaggerOperation(Summary = "Create a new Register Shift record")]
    [SwaggerResponse(StatusCodes.Status201Created, "Successful", typeof(RegisterShiftDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error Processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpPost]

    public async Task<IResult> Create([FromBody] CreateRegisterShiftCommand createRegisterShiftCommand)
    {
        var result = await mediator.Send(createRegisterShiftCommand);
        return result.Match(onSuccess => TypedResults.Created());
    }
}
