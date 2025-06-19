using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GetAllDashboardPowerBI;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Application.Capacity.Queries.GellAllCapacity;
using Poliedro.Eds.Application.Compartiment.Queries.GellAllCompartiment;

namespace Poliedro.Eds.Api.Controllers.v1.DashboardPowerBI;
    
[Route("api/v1/dashboard")]
[ApiController]
public class DashboardPowerBI(IMediator mediator) : ControllerBase
{
   
    [HttpGet("capacities")]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllCapacityQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }
        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }

    [HttpGet("Compartiment")]
    public async Task<ActionResult<IEnumerable<CompartimentDto>>> GetAll([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllCompartimentQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }

        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }
}

