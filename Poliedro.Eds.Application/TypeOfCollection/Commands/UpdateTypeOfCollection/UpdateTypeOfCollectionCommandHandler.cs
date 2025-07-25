using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.TypeOfCollection.DomainTypeOfCollection;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;
using System.Net;

namespace Poliedro.Eds.Application.TypeOfCollection.Commands.UpdateTypeOfCollection;

    public class UpdateTypeOfCollectionCommandHandler(
        ITypeOfCollectionUpdateTypeOfCollection TypeOfCollectionDomainTypeOfCollection,
        IMapper mapper,
        IValidator<UpdateTypeOfCollectionCommand> validator
        ) : IRequestHandler<UpdateTypeOfCollectionCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateTypeOfCollectionCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var TypeOfCollectionEntity = mapper.Map<TypeOfCollectionEntity>(request);
                var result = await TypeOfCollectionDomainTypeOfCollection.UpdateAsync(TypeOfCollectionEntity);

                if (!result.IsSuccess)
                    return result.Error!;

                return result.Value!;
        }
    }