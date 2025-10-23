using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.RegisterShift.Validations;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.RegisterShift.DomainService;
using Poliedro.Eds.Domain.RegisterShift.Entities;
using Poliedro.Eds.Domain.RegisterShift.Exceptions;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Application.RegisterShift.Dtos;
using FluentValidation.Results;

namespace Poliedro.Eds.Application.RegisterShift.Commands.CreateRegisterShift;

public class CreateRegisterShiftHandlerCommand(
    IRegisterShiftCreateService registerShiftCreateService,
    IMapper mapper,
    RegisterShiftCreateValidator validator) : IRequestHandler<CreateRegisterShiftCommand, RegisterShiftDto>
{
    public async Task<RegisterShiftDto> Handle(CreateRegisterShiftCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Request, cancellationToken);

        var entity = mapper.Map<RegisterShiftEntity>(request.Request);
        var result = await registerShiftCreateService.CreateAsync(entity, cancellationToken);
        if (!result.IsSuccess)
        {
            throw new RegisterShiftDomainException(result.Error?.ToString() ?? "Error al crear el turno.");
        }
        return mapper.Map<RegisterShiftDto>(entity);
    }       
}

