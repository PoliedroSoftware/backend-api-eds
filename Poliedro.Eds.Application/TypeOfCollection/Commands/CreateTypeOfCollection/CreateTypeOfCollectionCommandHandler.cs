using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.TypeOfCollection.DomainTypeOfCollection;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;
using System.Net;

namespace Poliedro.Eds.Application.TypeOfCollection.Commands.CreateTypeOfCollection;
    public class CreateTypeOfCollectionCommandHandler(
        ITypeOfCollectionCreateTypeOfCollection TypeOfCollectionDomainTypeOfCollection,
        IMapper mapper,
        IValidator<CreateTypeOfCollectionRequestDto> validator
        ) : IRequestHandler<CreateTypeOfCollectionCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateTypeOfCollectionCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var TypeOfCollectionEntity = mapper.Map<TypeOfCollectionEntity>(request.Request);
                var result = await TypeOfCollectionDomainTypeOfCollection.CreateAsync(TypeOfCollectionEntity);
                if (!result.IsSuccess)
                    return result.Error!;

                return result.Value!;
        }
    }