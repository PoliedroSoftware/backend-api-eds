using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Core;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Application.StrongBox.Validation;
using Poliedro.Eds.Domain.StrongBox.Services;

namespace Poliedro.Eds.Application.StrongBox.Commands
{
    public class StrongBoxCreateCommandHandler : IRequestHandler<StrongBoxCreateCommand, StrongBoxDto>
    {
        private readonly IStrongBoxService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<StrongBoxCreateRequest> _validator;

        public StrongBoxCreateCommandHandler(IStrongBoxService strongBoxService, IMapper mapper, StrongBoxCreateValidator validator)
        {
            _service = strongBoxService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<StrongBoxDto> Handle(StrongBoxCreateCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request.Request, cancellationToken);

            var created = await _service.CreateAsync(
                request.Request.DateTime,
                request.Request.IdCorte,
                request.Request.Type,
                request.Request.Ammount,
                request.Request.Note,
                request.Request.CreatedBy,
                cancellationToken);

            return _mapper.Map<StrongBoxDto>(created);
        }
    }
}
