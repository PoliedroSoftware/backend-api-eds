using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Bank.Commands;
using Poliedro.Eds.Application.Bank.Dtos;
using Poliedro.Eds.Application.Bank.Querys.BankGetAll;
using Poliedro.Eds.Application.Bank.Querys.BankGetById;
using Poliedro.Eds.Application.Bank.Querys.BankGetCurrentBalance;
using Poliedro.Eds.Application.Common.Features;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.Bank;

[Route("api/v1/bank")]
[ApiController]
public class BankController(IMediator mediator) : ControllerBase
{
    [SwaggerOperation(Summary = "Create new Bank entry")]
    [SwaggerResponse(StatusCodes.Status201Created, "The operation was successful.", typeof(BankDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BankDtoCreateRequest createBankRequest)
    {
        try
        {
            var command = new BankCreateCommand(createBankRequest);
            var result = await mediator.Send(command);
            
            return StatusCode(StatusCodes.Status201Created, 
                ResponseApiService.Response(StatusCodes.Status201Created, result));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ResponseApiService.Response(StatusCodes.Status500InternalServerError, ex.Message));
        }
    }

    [SwaggerOperation(Summary = "Get all Bank entries")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(IEnumerable<BankDto>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? idAccount = null, [FromQuery] int? idEds = null)
    {
        try
        {
            var query = new BankGetAllQuery(idAccount, idEds);
            var result = await mediator.Send(query);
            
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, result));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ResponseApiService.Response(StatusCodes.Status500InternalServerError, ex.Message));
        }
    }

    [SwaggerOperation(Summary = "Get Bank entry by ID")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(BankDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The specified Bank entry does not exist.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var query = new BankGetId(id);
            var result = await mediator.Send(query);
            
            if (result == null)
            {
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, 
                    "Bank entry not found."));
            }
            
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, result));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ResponseApiService.Response(StatusCodes.Status500InternalServerError, ex.Message));
        }
    }

    [SwaggerOperation(Summary = "Get current balance for Account ID")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(double))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpGet("current-balance/{accountId}")]
    public async Task<IActionResult> GetCurrentBalance([FromRoute] int accountId)
    {
        try
        {
            var query = new BankGetCurrentBalance(accountId);
            var result = await mediator.Send(query);
            
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, new { Balance = result, AccountId = accountId }));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ResponseApiService.Response(StatusCodes.Status500InternalServerError, ex.Message));
        }
    }
}
