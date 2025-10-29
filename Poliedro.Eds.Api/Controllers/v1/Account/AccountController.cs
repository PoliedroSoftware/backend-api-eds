using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Account.Commands.CreateAccount;
using Poliedro.Eds.Application.Account.Dtos;
using Poliedro.Eds.Application.Account.Queries.GetAccountById;
using Poliedro.Eds.Application.Account.Queries.GetAllAccounts;
using Poliedro.Eds.Application.Common.Features;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Account;

[Route("api/v1/account")]
[ApiController]
public class AccountController(IMediator mediator) : ControllerBase
{
    [SwaggerOperation(Summary = "Create new Account")]
    [SwaggerResponse(StatusCodes.Status201Created, "The operation was successful.", typeof(AccountDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AccountCreateDto createAccountRequest)
    {
        try
        {
            int? idEds = null;
            var claimVal = HttpContext.User?.FindFirst("id_eds")?.Value ?? HttpContext.User?.FindFirst("idEds")?.Value;
            if (!string.IsNullOrEmpty(claimVal) && int.TryParse(claimVal, out var parsedClaim))
                idEds = parsedClaim;
            else if (HttpContext.Items["id_eds"] != null && int.TryParse(HttpContext.Items["id_eds"]?.ToString(), out var parsedItem))
                idEds = parsedItem;

            var command = new CreateAccountCommand(createAccountRequest, idEds);
            var result = await mediator.Send(command);

            return ApiResponse(result, StatusCodes.Status201Created, StatusCodes.Status400BadRequest);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            return BadRequest(new ValidationProblemDetails(errors));
        }
        catch (Exception ex)
        {
            return Problem(title: "Internal Server Error",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [SwaggerOperation(Summary = "Get all accounts")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(IEnumerable<AccountDto>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No accounts found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllAccountsQuery();
            var result = await mediator.Send(query);

            return ApiResponse(result, StatusCodes.Status200OK, StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            return Problem(title: "Internal Server Error",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [SwaggerOperation(Summary = "Get Account by ID")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(AccountDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The specified Account does not exist.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var query = new GetAccountByIdQuery(id);
            var result = await mediator.Send(query);

            return ApiResponse(result, StatusCodes.Status200OK, StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            return Problem(title: "Internal Server Error",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private IActionResult ApiResponse<T>(T? data, int successStatus, int notFoundStatus)
    {
        if (data is null)
        {
            return StatusCode(notFoundStatus, ResponseApiService.Response(notFoundStatus, "Resource not found"));
        }

        if (data is System.Collections.IEnumerable seq && !(data is string))
        {
            var enumerator = seq.GetEnumerator();
            if (!enumerator.MoveNext())
                return StatusCode(notFoundStatus, ResponseApiService.Response(notFoundStatus, "No resources found"));
        }

        return StatusCode(successStatus, ResponseApiService.Response(successStatus, data));
    }
}
