using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos.CapacityDashboardView;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GetAllCapacityDashboardView;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1.PowerBI;

public static class CapacityDashboardViewEndpoints
{
    public static IEndpointRouteBuilder MapCapacityDashboardViewEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/powerbi/capacity")
            .WithTags("PowerBI - Dashboard");

        group.MapGet("", GetAll)
            .WithName("GetAllCapacityDashboardView")
            .WithSummary("Get all capacity view for PowerBI")
            .Produces<IEnumerable<CapacityDashboardViewDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> GetAll(
        [AsParameters] PaginationParams paginationParams,
        IMediator mediator)
    {
        var data = await mediator.Send(new GetAllCapacityDashboardViewQuery(new PaginationParams 
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
