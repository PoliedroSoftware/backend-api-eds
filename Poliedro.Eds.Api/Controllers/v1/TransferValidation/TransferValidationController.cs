using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.TransferValidation.Commands.CreateTransferValidation;
using Poliedro.Eds.Application.TransferValidation.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.TransferValidation;

[Route("api/v1/transfer-validation")]
[ApiController]
public class TransferValidationController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Crea una nueva validación de transferencia bancaria
    /// </summary>
    /// <param name="createRequest">Datos de la transferencia a validar</param>
    /// <returns>La validación de transferencia creada</returns>
    [SwaggerOperation(
        Summary = "Crear nueva validación de transferencia",
        Description = "Crea un nuevo registro de validación de transferencia bancaria en el sistema. " +
                      "Permite registrar transferencias con estados: PENDIENTE, CONFIRMADA o RECHAZADA.")]
    [SwaggerResponse(StatusCodes.Status201Created, 
        "La validación de transferencia fue creada exitosamente.", 
        typeof(TransferValidationDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, 
        "Los parámetros de la solicitud son incorrectos o la validación falló.", 
        typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, 
        "La solicitud carece de credenciales de autenticación válidas.", 
        typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status409Conflict, 
        "Ya existe una validación con el mismo ID único.", 
        typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, 
        "Error interno al procesar la solicitud.", 
        typeof(ProblemDetails))]

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] TransferValidationCreateRequestDto createRequest)
    {
        try
        {
            var command = new CreateTransferValidationCommand(createRequest);
            var result = await mediator.Send(command);

            return StatusCode(
                StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, result));
        }
        catch (ValidationException ex)
        {
            return BadRequest(
                ResponseApiService.Response(StatusCodes.Status400BadRequest, ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                ResponseApiService.Response(StatusCodes.Status500InternalServerError, ex.Message));
        }
    }
}
