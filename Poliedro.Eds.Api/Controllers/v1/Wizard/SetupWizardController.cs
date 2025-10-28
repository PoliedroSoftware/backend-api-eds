using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Wizard.Commands.CreateSetup;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Wizard
{
    [Route("api/v1/bootstrap/setup")]
    [ApiController]
    public class SetupWizardController(IMediator mediator) : ControllerBase
    {
        [SwaggerOperation(Summary = "Create new Setup")]
        [SwaggerResponse(StatusCodes.Status201Created, "The operation was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]

        public async Task<IResult> Create([FromBody] CreateSetupCommand createSetupCommand)
        {
            var result = await mediator.Send(createSetupCommand);
            return result.Match(onSuccess => TypedResults.Created());
        }
    }
}
