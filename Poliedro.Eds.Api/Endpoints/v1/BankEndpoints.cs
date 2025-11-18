using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Bank.Commands;
using Poliedro.Eds.Application.Bank.Dtos;
using Poliedro.Eds.Application.Bank.Querys.BankGetAll;
using Poliedro.Eds.Application.Bank.Querys.BankGetById;
using Poliedro.Eds.Application.Bank.Querys.BankGetCurrentBalance;
using Poliedro.Eds.Application.Common.Features;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class BankEndpoints
{
    public static IEndpointRouteBuilder MapBankEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/bank")
            .WithTags("Bank")
            .RequireAuthorization("AdminOrIslander");

        group.MapPost("", Create)
            .WithName("CreateBank")
            .WithSummary("Create new Bank entry")
            .Produces<BankDto>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        group.MapGet("", GetAll)
            .WithName("GetAllBanks")
            .WithSummary("Get all Bank entries")
            .Produces<IEnumerable<BankDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        group.MapGet("{id:int}", GetById)
            .WithName("GetBankById")
            .WithSummary("Get Bank entry by ID")
            .Produces<BankDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        group.MapGet("current-balance/{accountId:int}", GetCurrentBalance)
            .WithName("GetCurrentBalance")
            .WithSummary("Get current balance for Account ID")
            .Produces<object>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> Create(
        [FromBody] BankDtoCreateRequest createBankRequest,
        IMediator mediator)
    {
        try
        {
            var command = new BankCreateCommand(createBankRequest);
            var result = await mediator.Send(command);

            return TypedResults.Json(
                ResponseApiService.Response(StatusCodes.Status201Created, result),
                statusCode: StatusCodes.Status201Created);
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest(
                ResponseApiService.Response(StatusCodes.Status400BadRequest, ex.Errors));
        }
        catch (Exception ex)
        {
            return TypedResults.Json(
                ResponseApiService.Response(StatusCodes.Status500InternalServerError, ex.Message),
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> GetAll(
        [FromQuery] int? idAccount,
        [FromQuery] int? idEds,
        IMediator mediator)
    {
        try
        {
            var query = new BankGetAllQuery(idAccount, idEds);
            var result = await mediator.Send(query);

            return TypedResults.Ok(ResponseApiService.Response(StatusCodes.Status200OK, result));
        }
        catch (Exception ex)
        {
            return TypedResults.Json(
                ResponseApiService.Response(StatusCodes.Status500InternalServerError, ex.Message),
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> GetById(
        [FromRoute] int id,
        IMediator mediator)
    {
        try
        {
            var query = new BankGetId(id);
            var result = await mediator.Send(query);

            if (result == null)
            {
                return TypedResults.NotFound(
                    ResponseApiService.Response(StatusCodes.Status404NotFound, "Bank entry not found."));
            }

            return TypedResults.Ok(ResponseApiService.Response(StatusCodes.Status200OK, result));
        }
        catch (Exception ex)
        {
            return TypedResults.Json(
                ResponseApiService.Response(StatusCodes.Status500InternalServerError, ex.Message),
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> GetCurrentBalance(
        [FromRoute] int accountId,
        IMediator mediator)
    {
        try
        {
            var query = new BankGetCurrentBalance(accountId);
            var result = await mediator.Send(query);

            return TypedResults.Ok(
                ResponseApiService.Response(StatusCodes.Status200OK, new { Balance = result, AccountId = accountId }));
        }
        catch (Exception ex)
        {
            return TypedResults.Json(
                ResponseApiService.Response(StatusCodes.Status500InternalServerError, ex.Message),
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
