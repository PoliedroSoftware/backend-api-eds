using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos.CompartimentDashboardView;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GetAllCompartimentDashboardView;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1.PowerBI;

public static class CompartimentDashboardViewEndpoints
{
    public static IEndpointRouteBuilder MapCompartimentDashboardViewEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/powerbi/compartiment-dashboard")
            .WithTags("PowerBI - Dashboard");

        group.MapGet("", GetAll)
            .WithName("GetAllCompartimentDashboardView")
            .WithSummary("Get all compartiment view for PowerBI")
            .Produces<IEnumerable<CompartimentDashboardViewDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> GetAll(
        [AsParameters] PaginationParams paginationParams,
        IMediator mediator)
    {
        var data = await mediator.Send(new GetAllCompartimentDashboardViewQuery(new PaginationParams 
        { 
            PageNumber = paginationParams.PageNumber, 
            PageSize = paginationParams.PageSize 
        }));
        
        if (data is null || !data.Any())
        {
            return TypedResults.Json(
                ResponseApiService.Response(StatusCodes.Status404NotFound),
                statusCode: StatusCodes.Status404NotFound);
        }
        
        return TypedResults.Json(
            ResponseApiService.Response(StatusCodes.Status200OK, data),
            statusCode: StatusCodes.Status200OK);
    }
}
