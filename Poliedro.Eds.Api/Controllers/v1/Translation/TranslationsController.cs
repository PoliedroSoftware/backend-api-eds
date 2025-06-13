using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.Translations.Dtos;
using Poliedro.Eds.Application.Translations.Querys;

namespace Poliedro.Eds.Api.Controllers.v1.Translation;

[ApiController]
[Route("api/v1/translations")]
public class TranslationsController(IMediator _mediator) : ControllerBase
{
    [Authorize(Policy = "AdminOrIslander")]
    [HttpGet]
    public async Task<ActionResult<TranslationsAvailableDto>> GetAllTranslationsAsync()
    =>
        Ok(await _mediator.Send(new GetTranslationsQuery()));
    
}
