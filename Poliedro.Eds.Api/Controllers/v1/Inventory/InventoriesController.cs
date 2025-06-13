using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;
using Poliedro.Eds.Application.Inventory.Queries.GetInventoryList;
using Microsoft.AspNetCore.Authorization;

namespace Poliedro.Eds.Api.Controllers.v1.Inventory;

[Route("api/v1/inventory")]
[ApiController]
public class InventoryController(IMediator mediator) : ControllerBase
{
    [SwaggerOperation(Summary = "Get all Inventory")]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpGet]

    public async Task<IActionResult> GetInventoryAll([FromQuery] PaginationParams paginationParams)
    {
        var query = new GetInventoryListQuery(paginationParams);
        var result = await mediator.Send(query);
        return Ok(result);
    }



}