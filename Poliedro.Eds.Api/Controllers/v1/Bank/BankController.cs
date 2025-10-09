using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Bank.Commands;
using Poliedro.Eds.Application.Bank.Dtos;
using Poliedro.Eds.Application.Bank.Querys.BankGetById;
using Poliedro.Eds.Application.Bank.Querys.BankGetTotalBalance;
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
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
            }
            
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, result));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ResponseApiService.Response(StatusCodes.Status500InternalServerError, ex.Message));
        }
    }

    [SwaggerOperation(Summary = "Get Bank balance by Account ID")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(BankDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No bank entries found for the specified account.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpGet("balance/{accountId}")]
    public async Task<IActionResult> GetBalanceByAccount([FromRoute] int accountId)
    {
        try
        {
            var query = new BankGetTotalBalance(accountId);
            var result = await mediator.Send(query);
            
            if (result == null)
            {
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, 
                    "No bank entries found for the specified account."));
            }
            
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, result));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ResponseApiService.Response(StatusCodes.Status500InternalServerError, ex.Message));
        }
    }
}
