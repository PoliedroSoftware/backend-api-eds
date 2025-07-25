using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Capacity.Commands.CreateCapacity;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CompartimentCapacity.DomainCompartimentCapacity;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;
using System.Net;

namespace Poliedro.Eds.Application.CompartimentCapacity.Commands.CreateCompartimentCapacity;
    public class CreateCompartimentCapacityCommandHandler(
        ICompartimentCapacityCreateService CompartimentCapacityDomainCompartimentCapacity,
        IMapper mapper,
        IValidator<CreateCompartimentCapacityRequestDto> validator
        ) : IRequestHandler<CreateCompartimentCapacityCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateCompartimentCapacityCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var CompartimentCapacityEntity = mapper.Map<CompartimentCapacityEntity>(request.Request);
                var result = await CompartimentCapacityDomainCompartimentCapacity.CreateAsync(CompartimentCapacityEntity);
                if (!result.IsSuccess)
                    return result.Error!;

                return result.Value!;
        }
    }