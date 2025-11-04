using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.TransferValidation.Commands.CreateTransferValidation;
using Poliedro.Eds.Application.TransferValidation.Commands.UpdateTransferValidation;
using Poliedro.Eds.Application.TransferValidation.Dtos;
using Poliedro.Eds.Application.TransferValidation.Queries.GetAllTransferValidation;
using Poliedro.Eds.Domain.Common.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.TransferValidation;

[Route("api/v1/transfer-validation")]
[ApiController]
public class TransferValidationController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Obtiene todas las validaciones de transferencia con paginación
    /// </summary>
    /// <param name="paginationParams">Parámetros de paginación (PageNumber y PageSize)</param>
    /// <returns>Lista de validaciones de transferencia</returns>
    [SwaggerOperation(
    Summary = "Obtener todas las validaciones de transferencia",
        Description = "Obtiene una lista paginada de todas las validaciones de transferencia bancaria registradas en el sistema. " +
        "Los resultados están ordenados por fecha de creación (más recientes primero).")]
    [SwaggerResponse(StatusCodes.Status200OK,
        "La operación fue exitosa.",
        typeof(IEnumerable<TransferValidationDto>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Los parámetros de paginación son incorrectos.",
        typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized,
        "La solicitud carece de credenciales de autenticación válidas.",
        typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound,
        "No se encontraron validaciones de transferencia.",
        typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError,
        "Error interno al procesar la solicitud.",
 typeof(ProblemDetails))]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
    {
        try
        {
            var query = new GetAllTransferValidationQuery(
    new PaginationParams
    {
        PageNumber = paginationParams.PageNumber,
        PageSize = paginationParams.PageSize
    });

            var data = await mediator.Send(query);

            if (data is null || !data.Any())
            {
                return StatusCode(
           StatusCodes.Status404NotFound,
                      ResponseApiService.Response(StatusCodes.Status404NotFound, "No se encontraron validaciones de transferencia."));
            }

            return StatusCode(
               StatusCodes.Status200OK,
         ResponseApiService.Response(StatusCodes.Status200OK, data));
        }
        catch (Exception ex)
        {
            return StatusCode(
          StatusCodes.Status500InternalServerError,
        ResponseApiService.Response(StatusCodes.Status500InternalServerError, ex.Message));
        }
    }

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

    /// <summary>
    /// Actualiza una validación de transferencia bancaria existente
    /// </summary>
    /// <param name="id">ID de la validación de transferencia a actualizar</param>
    /// <param name="updateRequest">Datos actualizados de la transferencia</param>
    /// <returns>La validación de transferencia actualizada</returns>
    [SwaggerOperation(
        Summary = "Actualizar validación de transferencia",
        Description = "Actualiza un registro existente de validación de transferencia bancaria en el sistema. " +
          "El ID se pasa como parámetro de consulta y los datos a actualizar en el cuerpo de la petición.")]
    [SwaggerResponse(StatusCodes.Status200OK,
        "La validación de transferencia fue actualizada exitosamente.",
        typeof(TransferValidationDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
   "Los parámetros de la solicitud son incorrectos o la validación falló.",
        typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized,
      "La solicitud carece de credenciales de autenticación válidas.",
  typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound,
      "No se encontró la validación de transferencia con el ID especificado.",
        typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError,
        "Error interno al procesar la solicitud.",
        typeof(ProblemDetails))]
    [HttpPut]
    public async Task<IActionResult> Update(
        [FromQuery] int id,
        [FromBody] UpdateTransferValidationRequestDto updateRequest)
    {
        try
        {
            var command = new UpdateTransferValidationCommand(id, updateRequest);
            var result = await mediator.Send(command);

            return StatusCode(
     StatusCodes.Status200OK,
        ResponseApiService.Response(StatusCodes.Status200OK, result));
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
