using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Account.Commands.CreateAccount;
using Poliedro.Eds.Application.Account.Dtos;
using Poliedro.Eds.Application.Account.Queries.GetAccountById;
using Poliedro.Eds.Application.Account.Queries.GetAllAccounts;
using Poliedro.Eds.Application.Common.Features;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/account")
            .WithTags("Account")
            .RequireAuthorization("AdminOrIslander");

        group.MapPost("", Create)
            .WithName("CreateAccount")
            .WithSummary("Create new Account")
            .Produces<AccountDto>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        group.MapGet("", GetAll)
            .WithName("GetAllAccounts")
            .WithSummary("Get all accounts")
            .Produces<IEnumerable<AccountDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        group.MapGet("{id:int}", GetById)
            .WithName("GetAccountById")
            .WithSummary("Get Account by ID")
            .Produces<AccountDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> Create(
        [FromBody] AccountCreateDto createAccountRequest,
        IMediator mediator)
    {
        try
        {
            var command = new CreateAccountCommand(createAccountRequest);
            var result = await mediator.Send(command);

            return ApiResponse(result, StatusCodes.Status201Created, StatusCodes.Status400BadRequest);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            return TypedResults.BadRequest(new ValidationProblemDetails(errors));
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(
                title: "Internal Server Error",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> GetAll(IMediator mediator)
    {
        try
        {
            var query = new GetAllAccountsQuery();
            var result = await mediator.Send(query);

            return ApiResponse(result, StatusCodes.Status200OK, StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(
                title: "Internal Server Error",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> GetById(
        [FromRoute] int id,
        IMediator mediator)
    {
        try
        {
            var query = new GetAccountByIdQuery(id);
            var result = await mediator.Send(query);

            return ApiResponse(result, StatusCodes.Status200OK, StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(
                title: "Internal Server Error",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static IResult ApiResponse<T>(T? data, int successStatus, int notFoundStatus)
    {
        if (data is null)
        {
            return TypedResults.Json(
                ResponseApiService.Response(notFoundStatus, "Resource not found"),
                statusCode: notFoundStatus);
        }

        if (data is System.Collections.IEnumerable seq && !(data is string))
        {
            var enumerator = seq.GetEnumerator();
            if (!enumerator.MoveNext())
                return TypedResults.Json(
                    ResponseApiService.Response(notFoundStatus, "No resources found"),
                    statusCode: notFoundStatus);
        }

        return TypedResults.Json(
            ResponseApiService.Response(successStatus, data),
            statusCode: successStatus);
    }
}
