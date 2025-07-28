using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllCapacity;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllCompartiment;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using FluentValidation;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GetCourtList;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllEds;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GetInventoryList;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllProduct;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllProvider;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllShopping;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllTypeOfCollection;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllBusiness;
using Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllShoppingProductView;

namespace Poliedro.Eds.Api.Controllers.v1.DashboardPowerBI;

[Route("api/v1/dashboard")]
[ApiController]
public class DashboardPowerBI(IMediator mediator) : ControllerBase
{

    [HttpGet("capacities")]
    public async Task<IActionResult> GetAllCapacity([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllCapacityQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }
        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }

    [HttpGet("compartiments")]
    public async Task<ActionResult<IEnumerable<CompartimentDto>>> GetAllcompartiments([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllCompartimentQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }

        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }

    [HttpGet("courts")]
    public async Task<IEnumerable<Application.DashboardPowerBI.Dtos.Court.CourtListResponseDto>> GetAllCourts([FromQuery] PaginationParams paginationParams, [FromServices] IValidator<GetCourtsListQuery> validator)
    {
        var getCourtsQuery = new GetCourtsListQuery(paginationParams);

        return await mediator.Send(getCourtsQuery);
    }

    [HttpGet("eds")]
    public async Task<IActionResult> GetAllEds([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllEdsQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }
        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }

    [HttpGet("inventories")]

    public async Task<IActionResult> GetInventoryAll([FromQuery] PaginationParams paginationParams)
    {
        var query = new GetInventoryListQuery(paginationParams);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetAllProducts([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllProductQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }
        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }

    [HttpGet("providers")]
    public async Task<IActionResult> GetAllProviders([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllProviderQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }
        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }


    [HttpGet("shoppings")]
    public async Task<ActionResult<IEnumerable<ShoppingDto>>> GetAllShoppings([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllShoppingQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }

        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }


    [HttpGet("typeofcollection")]
    public async Task<IActionResult> GetAllTypeOfCollection([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllTypeOfCollectionQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }
        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }

    [HttpGet("business")]
    public async Task<IActionResult> GetAllBusiness([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllBusinessQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }
        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }

    [HttpGet("shopping-products")]
    public async Task<ActionResult<IEnumerable<ShoppingProductViewDto>>> GetAll([FromQuery] PaginationParams paginationParams)
    {
        var data = await mediator.Send(new GellAllShoppingProductViewQuery(new PaginationParams { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        if (data is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
        }

        return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));
    }
}

