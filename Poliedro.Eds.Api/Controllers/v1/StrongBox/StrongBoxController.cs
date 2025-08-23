using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetList;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Controllers.v1.StrongBox
{
    [Route("api/v1/strongbox")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class  StrongBoxController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] long? idCorte = null,
            [FromQuery] string? type = null,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null)
        {
            var query = new StrongBoxGetList(Page: paginationParams.PageNumber,
                PageSize: paginationParams.PageSize,
                IdCorte: idCorte,
                Type: type,
                From: from,
                To: to);

            var data = await mediator.Send(query);

            if (data is null || data.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data));

        }
        
    }
}

