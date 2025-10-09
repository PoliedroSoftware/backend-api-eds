using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Account.Dtos;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Account.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Account;

[Route("api/v1/account")]
[ApiController]
public class AccountController(IAccountGetAllService accountGetAllService) : ControllerBase
{
    [SwaggerOperation(Summary = "Get all accounts")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(IEnumerable<AccountDto>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var accounts = await accountGetAllService.GetAllAsync();
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, accounts));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ResponseApiService.Response(StatusCodes.Status500InternalServerError, ex.Message));
        }
    }
}
